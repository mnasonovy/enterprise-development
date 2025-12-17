using AutoMapper;
using Library.Application.Contracts.Issues;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления выданными книгами (Issue).
/// Реализует CRUD операции с выданными книгами.
/// </summary>
public class IssueService : IIssueService
{
    private readonly IssueRepository _issueRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<IssueService> _logger;

    public IssueService(IssueRepository issueRepository, IMapper mapper, ILogger<IssueService> logger)
    {
        _issueRepository = issueRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить выданную книгу по ID.
    /// Возвращает null если не найдена.
    /// </summary>
    public async Task<IssueDto?> GetAsync(int id)
    {
        _logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);

        var issue = await _issueRepository.GetAsync(id);

        if (issue is null)
        {
            _logger.LogWarning("{Method} Issue not found with id = {Id}", nameof(GetAsync), id);
            return null;
        }

        var result = _mapper.Map<IssueDto>(issue);
        _logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));

        return result;
    }

    /// <summary>
    /// Получить все выданные книги.
    /// </summary>
    public async Task<IReadOnlyList<IssueDto>> GetListAsync()
    {
        _logger.LogInformation("{Method} method is called", nameof(GetListAsync));

        var issues = await _issueRepository.GetListAsync();
        var result = _mapper.Map<IReadOnlyList<IssueDto>>(issues);

        _logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);

        return result;
    }

    /// <summary>
    /// Создать новую выдачу книги.
    /// Требуется явно указать Id, BookId, ReaderId, IssueDate, DaysCount.
    /// </summary>
    public async Task<IssueDto> CreateAsync(IssueCreateUpdateDto input)
    {
        _logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input is null)
            throw new ArgumentException("Issue data is required");

        if (input.Id <= 0)
            throw new ArgumentException("Issue ID must be greater than 0");

        if (input.BookId <= 0)
            throw new ArgumentException("Book ID must be greater than 0");

        if (input.ReaderId <= 0)
            throw new ArgumentException("Reader ID must be greater than 0");

        if (input.IssueDate == default)
            throw new ArgumentException("Issue date is required");

        if (input.DaysCount <= 0)
            throw new ArgumentException("Days count must be greater than 0");

        var issue = new Issue
        {
            Id = input.Id,
            BookId = input.BookId,
            ReaderId = input.ReaderId,
            IssueDate = input.IssueDate,
            DaysCount = input.DaysCount,
            ReturnDate = input.ReturnDate,
            Book = null!,
            Reader = null!
        };

        var created = await _issueRepository.CreateAsync(issue);

        _logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), created.Id);

        return _mapper.Map<IssueDto>(created);
    }

    /// <summary>
    /// Обновить выданную книгу.
    /// Используется для отметки возврата (установка ReturnDate).
    /// </summary>
    public async Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input)
    {
        _logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (input is null)
            throw new ArgumentException("Issue data is required");

        if (input.BookId <= 0)
            throw new ArgumentException("Book ID must be greater than 0");

        if (input.ReaderId <= 0)
            throw new ArgumentException("Reader ID must be greater than 0");

        if (input.IssueDate == default)
            throw new ArgumentException("Issue date is required");

        if (input.DaysCount <= 0)
            throw new ArgumentException("Days count must be greater than 0");

        var existing = await _issueRepository.GetAsync(id);

        if (existing is null)
        {
            _logger.LogWarning("{Method} Issue not found with id = {Id}", nameof(UpdateAsync), id);
            return null;
        }

        existing.BookId = input.BookId;
        existing.ReaderId = input.ReaderId;
        existing.IssueDate = input.IssueDate;
        existing.DaysCount = input.DaysCount;
        existing.ReturnDate = input.ReturnDate;

        var updated = await _issueRepository.UpdateAsync(existing);

        _logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

        return updated is null ? null : _mapper.Map<IssueDto>(updated);
    }

    /// <summary>
    /// Удалить запись о выданной книге.
    /// Книга и читатель остаются в системе.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("{Method} method is called with id = {Id}", nameof(DeleteAsync), id);

        await _issueRepository.DeleteAsync(id);

        _logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));
    }
}
