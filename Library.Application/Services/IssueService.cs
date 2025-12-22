using AutoMapper;
using Library.Application.Contracts.Issues;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над выданными книгами (Issue).
/// Реализует интерфейс IIssueService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторю через интерфейс.
/// Валидация данных осуществляется на уровне контроллера через DataAnnotations.
/// </summary>
public class IssueService(IIssueRepository issueRepository, IMapper mapper) : IIssueService
{
    /// <summary>
    /// Получить выданную книгу по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выданной книги</param>
    /// <returns>IssueDto или null если запись не найдена</returns>
    public async Task<IssueDto?> GetAsync(int id)
    {
        var issue = await issueRepository.GetAsync(id);
        return issue == null ? null : mapper.Map<IssueDto>(issue);
    }

    /// <summary>
    /// Получить список всех выданных книг.
    /// </summary>
    /// <returns>Неизменяемый список IssueDto всех выданных книг</returns>
    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        var issues = await issueRepository.GetListAsync();
        return mapper.Map<IReadOnlyList<IssueDto>>(issues);
    }

    /// <summary>
    /// Создать новую выданную книгу.
    /// </summary>
    /// <param name="input">DTO с данными новой выданной книги</param>
    /// <returns>IssueDto созданной записи с заполненным Id</returns>
    public async Task<IssueDto> CreateAsync(IssueCreateUpdateDto input)
    {
        var issue = mapper.Map<Issue>(input);
        var created = await issueRepository.CreateAsync(issue);
        return mapper.Map<IssueDto>(created);
    }

    /// <summary>
    /// Обновить существующую выданную книгу.
    /// </summary>
    /// <param name="id">Уникальный идентификатор для обновления</param>
    /// <param name="input">DTO с новыми данными выданной книги</param>
    /// <returns>IssueDto обновленной записи или null если не найдена</returns>
    public async Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input)
    {
        var existing = await issueRepository.GetAsync(id);
        if (existing == null)
            return null;

        mapper.Map(input, existing);
        var updated = await issueRepository.UpdateAsync(existing);
        return updated == null ? null : mapper.Map<IssueDto>(updated);
    }

    /// <summary>
    /// Удалить выданную книгу по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await issueRepository.DeleteAsync(id);
    }
}