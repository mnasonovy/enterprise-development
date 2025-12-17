using AutoMapper;
using Library.Application.Contracts.Issues;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления выданными книгами (Issue).
/// Реализует интерфейс IIssueService, обеспечивая выполнение CRUD операций над выданными книгами.
/// Использует AutoMapper для преобразования между Domain моделями и DTO.
/// </summary>
public class IssueService : IIssueService
{
    private readonly IssueRepository _issueRepository;
    private readonly IMapper _mapper;

    public IssueService(IssueRepository issueRepository, IMapper mapper)
    {
        _issueRepository = issueRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает выданную книгу по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор выдачи для поиска.</param>
    /// <returns>DTO выдачи, если найдена; null если запись не существует.</returns>
    public async Task<IssueDto?> GetAsync(int id)
    {
        var issue = await _issueRepository.ReadAsync(id);
        return issue == null ? null : _mapper.Map<IssueDto>(issue);
    }

    /// <summary>
    /// Получает список всех выданных книг.
    /// </summary>
    /// <returns>Коллекция DTO всех выданных книг. Если выданных книг нет, возвращает пустой список.</returns>
    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        var issues = await _issueRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<IssueDto>>(issues);
    }

    /// <summary>
    /// Создает новую запись о выдаче книги.
    /// </summary>
    /// <param name="input">DTO с данными выдачи (BookId, ReaderId, IssueDate, DaysCount обязательны).</param>
    /// <returns>DTO созданной выдачи с назначенным идентификатором.</returns>
    public async Task<IssueDto> CreateAsync(IssueCreateUpdateDto input)
    {
        var issue = _mapper.Map<Issue>(input);
        // Устанавливаем только внешние ключи
        issue.BookId = input.BookId;
        issue.ReaderId = input.ReaderId;
        issue.IssueDate = input.IssueDate;
        issue.DaysCount = input.DaysCount;
        issue.ReturnDate = null; // Книга еще не возвращена

        var created = await _issueRepository.CreateAsync(issue);
        return _mapper.Map<IssueDto>(created);
    }

    /// <summary>
    /// Обновляет информацию об существующей выданной книге.
    /// </summary>
    /// <param name="id">Идентификатор выдачи для обновления.</param>
    /// <param name="input">DTO с новыми данными выдачи.</param>
    /// <returns>Обновленный DTO выдачи, если успешно; null если запись не найдена.</returns>
    public async Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input)
    {
        var existing = await _issueRepository.ReadAsync(id);
        if (existing == null)
            return null;

        // Обновляем только базовые поля и внешние ключи
        existing.BookId = input.BookId;
        existing.ReaderId = input.ReaderId;
        existing.IssueDate = input.IssueDate;
        existing.DaysCount = input.DaysCount;

        var updated = await _issueRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<IssueDto>(updated);
    }

    /// <summary>
    /// Отмечает книгу как возвращенную путем установки даты возврата.
    /// </summary>
    /// <param name="id">Идентификатор выдачи для отметки возврата.</param>
    /// <returns>Обновленный DTO выдачи, если успешно; null если запись не найдена.</returns>
    public async Task<IssueDto?> MarkAsReturnedAsync(int id)
    {
        var existing = await _issueRepository.ReadAsync(id);
        if (existing == null)
            return null;

        existing.ReturnDate = DateTime.UtcNow;
        var updated = await _issueRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<IssueDto>(updated);
    }

    /// <summary>
    /// Удаляет запись о выданной книге из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор выдачи для удаления.</param>
    public async Task DeleteAsync(int id)
    {
        await _issueRepository.DeleteAsync(id);
    }
}
