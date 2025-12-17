using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Publishers;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

/// <summary>
/// Сервис для аналитических запросов библиотеки.
/// Агрегирует данные и выполняет сложные LINQ-запросы.
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IBookService _bookService;
    private readonly IIssueService _issueService;
    private readonly IReaderService _readerService;
    private readonly IPublisherService _publisherService;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(
        IBookService bookService,
        IIssueService issueService,
        IReaderService readerService,
        IPublisherService publisherService,
        ILogger<AnalyticsService> logger)
    {
        _bookService = bookService;
        _issueService = issueService;
        _readerService = readerService;
        _publisherService = publisherService;
        _logger = logger;
    }

    /// <summary>
    /// Получить все выданные книги в алфавитном порядке (без дубликатов).
    /// </summary>
    public async Task<IReadOnlyList<string>> GetIssuedBooksOrderedByTitleAsync()
    {
        _logger.LogInformation("{Method} method is called", nameof(GetIssuedBooksOrderedByTitleAsync));

        var issues = await _issueService.GetListAsync();

        var issuedBooks = issues
            .Select(i => i.BookTitle)
            .Distinct()
            .OrderBy(title => title)
            .ToList()
            .AsReadOnly();

        _logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetIssuedBooksOrderedByTitleAsync), issuedBooks.Count);

        return issuedBooks;
    }

    /// <summary>
    /// Получить топ 5 читателей за последние 6 месяцев.
    /// </summary>
    public async Task<IReadOnlyList<TopReaderDto>> GetTopReadersAsync()
    {
        _logger.LogInformation("{Method} method is called", nameof(GetTopReadersAsync));

        var readers = await _readerService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var last6Months = DateTime.UtcNow.AddMonths(-6);

        var topReaders = issues
            .Where(i => i.IssueDate >= last6Months)
            .GroupBy(i => i.ReaderId)
            .Select(g => new { ReaderId = g.Key, CountBooks = g.Count() })
            .OrderByDescending(x => x.CountBooks)
            .Take(5)
            .Select(x => new TopReaderDto
            {
                FullName = readers.FirstOrDefault(r => r.Id == x.ReaderId)?.FullName ?? "Unknown",
                CountBooks = x.CountBooks
            })
            .ToList()
            .AsReadOnly();

        _logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopReadersAsync), topReaders.Count);

        return topReaders;
    }

    /// <summary>
    /// Получить всех читателей со статистикой по дням выданных книг.
    /// Отсортировано по имени.
    /// </summary>
    public async Task<IReadOnlyList<ReaderDaysCountDto>> GetReadersByDaysCountAsync()
    {
        _logger.LogInformation("{Method} method is called", nameof(GetReadersByDaysCountAsync));

        var readers = await _readerService.GetListAsync();
        var issues = await _issueService.GetListAsync();

        var readersDaysCounts = issues
            .GroupBy(i => i.ReaderId)
            .Select(g => new ReaderDaysCountDto
            {
                FullName = readers.FirstOrDefault(r => r.Id == g.Key)?.FullName ?? "Unknown",
                CountDays = g.Sum(i => i.DaysCount)
            })
            .OrderBy(x => x.FullName)
            .ToList()
            .AsReadOnly();

        _logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetReadersByDaysCountAsync), readersDaysCounts.Count);

        return readersDaysCounts;
    }

    /// <summary>
    /// Получить топ 5 издательств за последний год.
    /// </summary>
    public async Task<IReadOnlyList<TopPublisherDto>> GetTopPublishersLastYearAsync()
    {
        _logger.LogInformation("{Method} method is called", nameof(GetTopPublishersLastYearAsync));

        var books = await _bookService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var lastYear = DateTime.UtcNow.AddYears(-1);

        var topPublishers = issues
            .Where(i => i.IssueDate >= lastYear)
            .GroupBy(i => i.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .Join(books, issue => issue.BookId, book => book.Id,
                (issue, book) => new { book.PublisherName, issue.Count })
            .GroupBy(x => x.PublisherName)
            .OrderByDescending(g => g.Sum(x => x.Count))
            .Take(5)
            .Select(g => new TopPublisherDto
            {
                PublisherName = g.Key ?? "Unknown",
                CountBooks = g.Sum(x => x.Count)
            })
            .ToList()
            .AsReadOnly();

        _logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopPublishersLastYearAsync), topPublishers.Count);

        return topPublishers;
    }

    /// <summary>
    /// Получить топ 5 наименее популярных книг за последний год.
    /// </summary>
    public async Task<IReadOnlyList<TopBookDto>> GetTopPopularBooksLastYearAsync()
    {
        _logger.LogInformation("{Method} method is called", nameof(GetTopPopularBooksLastYearAsync));

        var books = await _bookService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var lastYear = DateTime.UtcNow.AddYears(-1);

        var leastPopularBooks = issues
            .Where(i => i.IssueDate >= lastYear)
            .GroupBy(i => i.BookId)
            .Select(g => new { BookId = g.Key, TimesIssued = g.Count() })
            .OrderBy(x => x.TimesIssued)
            .Take(5)
            .Select(x => new TopBookDto
            {
                Title = books.FirstOrDefault(b => b.Id == x.BookId)?.Title ?? "Unknown",
                TimesIssued = x.TimesIssued
            })
            .ToList()
            .AsReadOnly();

        _logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopPopularBooksLastYearAsync), leastPopularBooks.Count);

        return leastPopularBooks;
    }
}
