using AutoMapper;
using Library.Application.Contracts.Issues;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над выданными книгами (Issue).
/// 🔧 ИСПРАВЛЕНО: AutoMapper + правильная работа с FK!
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
    /// Получить выданную книгу по идентификатору.
    /// 🔧 Include загружает Book и Reader!
    /// </summary>
    public async Task<IssueDto?> GetAsync(int id)
    {
        var issue = await _issueRepository.ReadAsync(id);
        return issue == null ? null : _mapper.Map<IssueDto>(issue);
    }

    /// <summary>
    /// Получить список всех выданных книг.
    /// 🔧 Include загружает Book и Reader!
    /// </summary>
    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        var issues = await _issueRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<IssueDto>>(issues);
    }

    /// <summary>
    /// Создать новую выданную книгу.
    /// 🔧 ИСПРАВЛЕНО: Используются FK вместо создания новых объектов!
    /// </summary>
    public async Task<IssueDto> CreateAsync(IssueCreateUpdateDto input)
    {
        var issue = _mapper.Map<Issue>(input);
        // 🔧 ВАЖНО: Устанавливаем только FK!
        issue.BookId = input.BookId;
        issue.ReaderId = input.ReaderId;
        issue.IssueDate = input.IssueDate;
        issue.DaysCount = input.DaysCount;
        issue.ReturnDate = null; // Книга еще не возвращена

        var created = await _issueRepository.CreateAsync(issue);
        return _mapper.Map<IssueDto>(created);
    }

    /// <summary>
    /// Обновить выданную книгу.
    /// 🔧 ИСПРАВЛЕНО: Используются FK вместо создания новых объектов!
    /// </summary>
    public async Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input)
    {
        var existing = await _issueRepository.ReadAsync(id);
        if (existing == null)
            return null;

        // 🔧 Обновляем только базовые поля и FK!
        existing.BookId = input.BookId;
        existing.ReaderId = input.ReaderId;
        existing.IssueDate = input.IssueDate;
        existing.DaysCount = input.DaysCount;

        var updated = await _issueRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<IssueDto>(updated);
    }

    /// <summary>
    /// Отметить книгу как возвращённую.
    /// 🔧 СПЕЦИАЛЬНЫЙ метод для отметки возврата!
    /// </summary>
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
    /// Удалить выданную книгу.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _issueRepository.DeleteAsync(id);
    }
}
