using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Publishers;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения аналитических запросов по библиотеке.
/// Реализует интерфейс IAnalyticsService, предоставляя сложные аналитические операции
/// с данными о книгах, читателях, издателях и выданных книгах.
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IBookService _bookService;
    private readonly IIssueService _issueService;
    private readonly IReaderService _readerService;
    private readonly IPublisherService _publisherService;

    public AnalyticsService(
        IBookService bookService,
        IIssueService issueService,
        IReaderService readerService,
        IPublisherService publisherService)
    {
        _bookService = bookService;
        _issueService = issueService;
        _readerService = readerService;
        _publisherService = publisherService;
    }

    /// <summary>
    /// Получить информацию о выданных книгах, упорядоченные по названию.
    /// </summary>
    /// <returns>Список всех выданных книг отсортированный по названию книги</returns>
    public async Task<IReadOnlyList<IssueDto>> GetIssuedBooksOrderedByTitleAsync()
    {
        var issues = await _issueService.GetListAsync();
        var issuedBooks = issues
            .OrderBy(i => i.BookTitle)
            .ToList()
            .AsReadOnly();
        return issuedBooks;
    }

    /// <summary>
    /// Получить топ N читателей, которые взяли больше всего книг в заданный период.
    /// </summary>
    /// <param name="from">Начало периода поиска</param>
    /// <param name="to">Конец периода поиска</param>
    /// <param name="topCount">Количество читателей в топе (по умолчанию 5)</param>
    /// <returns>Список DTO читателей с наибольшим количеством выданных книг в периоде</returns>
    public async Task<IReadOnlyList<ReaderDto>> GetTopReadersByPeriodAsync(DateTime from, DateTime to, int topCount = 5)
    {
        var readers = await _readerService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var topReaders = issues
            .Where(i => i.IssueDate >= from && i.IssueDate <= to)
            .GroupBy(i => i.ReaderId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => readers.FirstOrDefault(r => r.Id == g.Key))
            .Where(r => r != null)
            .ToList()
            .AsReadOnly();
        return topReaders!;
    }

    /// <summary>
    /// Получить читателей, которые брали книги на самый длительный период, упорядоченных по полному имени.
    /// </summary>
    /// <returns>Список DTO читателей упорядоченный по полному имени с максимальным средним периодом выдачи</returns>
    public async Task<IReadOnlyList<ReaderDto>> GetReadersWithLongestIssuePeriodAsync()
    {
        var readers = await _readerService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var readersWithLongestPeriod = issues
            .GroupBy(i => i.ReaderId)
            .Select(g => new
            {
                ReaderId = g.Key,
                AverageDays = g.Average(i => i.DaysCount)
            })
            .OrderByDescending(x => x.AverageDays)
            .Select(x => readers.FirstOrDefault(r => r.Id == x.ReaderId))
            .Where(r => r != null)
            .OrderBy(r => r!.FullName)
            .ToList()
            .AsReadOnly();
        return readersWithLongestPeriod!;
    }

    /// <summary>
    /// Получить топ N наиболее популярных издателей за последний год.
    /// </summary>
    /// <param name="topCount">Количество издателей в топе (по умолчанию 5)</param>
    /// <returns>Список DTO издателей упорядоченный по количеству выданных книг в убывающем порядке</returns>
    public async Task<IReadOnlyList<PublisherDto>> GetTopPublishersLastYearAsync(int topCount = 5)
    {
        var publishers = await _publisherService.GetListAsync();
        var books = await _bookService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var lastYear = DateTime.Now.AddYears(-1);
        var topPublishers = issues
            .Where(i => i.IssueDate >= lastYear)
            .GroupBy(i => i.BookId)
            .Select(g => new
            {
                BookId = g.Key,
                Count = g.Count()
            })
            .Join(books,
                issue => issue.BookId,
                book => book.Id,
                (issue, book) => new { book.PublisherName, issue.Count })
            .GroupBy(x => x.PublisherName)
            .OrderByDescending(g => g.Sum(x => x.Count))
            .Take(topCount)
            .Select(g => publishers.FirstOrDefault(p => p.Name == g.Key))
            .Where(p => p != null)
            .ToList()
            .AsReadOnly();
        return topPublishers!;
    }

    /// <summary>
    /// Получить топ N наименее популярных книг за последний год.
    /// </summary>
    /// <param name="topCount">Количество книг в топе (по умолчанию 5)</param>
    /// <returns>Список DTO книг упорядоченный по количеству выданных копий в возрастающем порядке</returns>
    public async Task<IReadOnlyList<BookDto>> GetLeastPopularBooksLastYearAsync(int topCount = 5)
    {
        var books = await _bookService.GetListAsync();
        var issues = await _issueService.GetListAsync();
        var lastYear = DateTime.Now.AddYears(-1);
        var leastPopularBooks = books
            .Select(b => new
            {
                Book = b,
                IssueCount = issues
                    .Where(i => i.BookId == b.Id && i.IssueDate >= lastYear)
                    .Count()
            })
            .OrderBy(x => x.IssueCount)
            .Take(topCount)
            .Select(x => x.Book)
            .ToList()
            .AsReadOnly();
        return leastPopularBooks;
    }
}
