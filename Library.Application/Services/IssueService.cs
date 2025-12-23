using AutoMapper;
using Library.Application.Contracts.Issues;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления выдачами (выданными книгами) в библиотечной системе.
/// Реализует CRUD операции и Upsert для синхронизации с NATS JetStream.
/// Выдача отслеживает процесс: Книга → Читатель → Возврат.
/// </summary>
public class IssueService(IIssueRepository issueRepository, IMapper mapper) : IIssueService
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получает информацию о выданной книге по идентификатору.
    /// </summary>
    public async Task<IssueDto?> GetAsync(int id)
    {
        var issue = await _issueRepository.GetAsync(id);
        return issue == null ? null : _mapper.Map<IssueDto>(issue);
    }

    /// <summary>
    /// Получает список всех выданных книг.
    /// </summary>
    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        var issues = await _issueRepository.GetListAsync();
        return _mapper.Map<IReadOnlyList<IssueDto>>(issues);
    }

    /// <summary>
    /// Создаёт новую запись о выданной книге.
    /// Требует существующих BookId и ReaderId.
    /// </summary>
    public async Task<IssueDto> CreateAsync(IssueCreateUpdateDto input)
    {
        var issue = _mapper.Map<Issue>(input);
        var created = await _issueRepository.CreateAsync(issue);
        return _mapper.Map<IssueDto>(created);
    }

    /// <summary>
    /// Обновляет информацию о выданной книге.
    /// Основное назначение - фиксация даты возврата.
    /// </summary>
    public async Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input)
    {
        var existing = await _issueRepository.GetAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _issueRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<IssueDto>(updated);
    }

    /// <summary>
    /// Удаляет запись о выданной книге по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _issueRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Создаёт новую выдачу или обновляет существующую (Upsert).
    /// Идемпотентная операция для синхронизации из NATS.
    /// </summary>
    public async Task<IssueDto> UpsertAsync(IssueCreateUpdateDto input)
    {
        var existing = await _issueRepository.GetAsync(input.Id);

        if (existing != null)
        {
            _mapper.Map(input, existing);
            var updated = await _issueRepository.UpdateAsync(existing);
            return _mapper.Map<IssueDto>(updated)!;
        }
        else
        {
            var issue = _mapper.Map<Issue>(input);
            var created = await _issueRepository.CreateAsync(issue);
            return _mapper.Map<IssueDto>(created);
        }
    }
}
