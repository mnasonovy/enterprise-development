namespace Library.Application.Services;

using Library.Application.Contracts.Issues;

using Library.Domain.Models;

using Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Сервис для CRUD-операций над проблемами/изданиями
/// </summary>
public class IssueService : IIssueService
{
    private readonly IssueRepository _issueRepository;

    public IssueService(IssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    /// <summary>
    /// Получить проблему по идентификатору
    /// </summary>
    public async Task<IssueDto?> GetAsync(int id)
    {
        var issue = await _issueRepository.ReadAsync(id);
        return issue is null
            ? null
            : MapToDto(issue);
    }

    /// <summary>
    /// Получить список всех проблем
    /// </summary>
    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        var issues = await _issueRepository.ReadAllAsync();
        return issues.Select(MapToDto).ToList().AsReadOnly();
    }

    /// <summary>
    /// Создать новую проблему
    /// </summary>
    public async Task<IssueDto> CreateAsync(IssueCreateUpdateDto input)
    {
        var issue = new Issue
        {
            Book = new Book { Id = input.BookId, Title = string.Empty, Year = 0, AlphabetCode = null!, BookType = null!, Publisher = null!, Authors = new() },
            Reader = new Reader { Id = input.ReaderId, FullName = string.Empty, RegistrationDate = input.IssueDate },
            IssueDate = input.IssueDate,
            DaysCount = input.DaysCount
        };

        var created = await _issueRepository.CreateAsync(issue);
        return MapToDto(created);
    }

    /// <summary>
    /// Обновить существующую проблему
    /// </summary>
    public async Task<IssueDto> UpdateAsync(int id, IssueCreateUpdateDto input)
    {
        var existing = await _issueRepository.ReadAsync(id)
            ?? throw new InvalidOperationException($"Issue with id {id} was not found.");

        existing.Book = new Book { Id = input.BookId, Title = string.Empty, Year = 0, AlphabetCode = null!, BookType = null!, Publisher = null!, Authors = new() };
        existing.Reader = new Reader { Id = input.ReaderId, FullName = string.Empty, RegistrationDate = input.IssueDate };
        existing.IssueDate = input.IssueDate;
        existing.DaysCount = input.DaysCount;

        var updated = await _issueRepository.UpdateAsync(existing)
            ?? throw new InvalidOperationException($"Issue with id {id} was not updated.");

        return MapToDto(updated);
    }

    /// <summary>
    /// Удалить проблему по идентификатору
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var deleted = await _issueRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Issue with id {id} was not deleted.");
        }
    }

    /// <summary>
    /// Преобразовать Domain модель проблемы в DTO
    /// </summary>
    private static IssueDto MapToDto(Issue issue)
    {
        return new IssueDto
        {
            Id = issue.Id,
            BookId = issue.Book.Id,
            BookTitle = issue.Book.Title,
            ReaderId = issue.Reader.Id,
            ReaderFullName = issue.Reader.FullName,
            IssueDate = issue.IssueDate,
            DaysCount = issue.DaysCount
        };
    }
}