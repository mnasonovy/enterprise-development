using Library.Application.Contracts.Issues;
using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Repositories;

namespace Library.Application.Services;

public class IssueService : IIssueService
{
    private readonly IssueMongoRepository _issueRepository;

    public IssueService(IssueMongoRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<IssueDto?> GetAsync(int id)
    {
        var issue = await _issueRepository.ReadAsync(id);
        return issue is null
            ? null
            : MapToDto(issue);
    }

    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        var issues = await _issueRepository.ReadAllAsync();
        return issues.Select(MapToDto).ToArray();
    }

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

    public async Task DeleteAsync(int id)
    {
        var deleted = await _issueRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Issue with id {id} was not deleted.");
        }
    }
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
