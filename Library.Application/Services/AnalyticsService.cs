using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Publishers;

namespace Library.Application.Services;

/// <summary>
/// Сервис для аналитических запросов (Analytics).
/// 🔧 ИСПРАВЛЕНО: Правильное использование async/await!
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
    /// 1. Получить информацию о выданных книгах, упорядоченные по названию.
    /// </summary>
    public async Task<IReadOnlyList<IssueDto>> GetIssuedBooksOrderedByTitleAsync()
    {
        var issues = await _issueService.GetListAsync(); // 🔧 ДОБАВЛЕНО await
        var issuedBooks = issues
            .OrderBy(i => i.BookTitle)
            .ToList()
            .AsReadOnly();

        return issuedBooks;
    }

    /// <summary>
    /// 2. Получить топ читателей, которые взяли больше всего книг в период.
    /// </summary>
    public async Task<IReadOnlyList<ReaderDto>> GetTopReadersByPeriodAsync(DateTime from, DateTime to, int topCount = 5)
    {
        var readers = await _readerService.GetListAsync(); // 🔧 ДОБАВЛЕНО await
        var issues = await _issueService.GetListAsync(); // 🔧 ДОБАВЛЕНО await

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
    /// 3. Получить читателей, которые брали книги на самый длинный период.
    /// </summary>
    public async Task<IReadOnlyList<ReaderDto>> GetReadersWithLongestIssuePeriodAsync()
    {
        var readers = await _readerService.GetListAsync(); // 🔧 ДОБАВЛЕНО await
        var issues = await _issueService.GetListAsync(); // 🔧 ДОБАВЛЕНО await

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
    /// 4. Получить топ издателей за последний год.
    /// </summary>
    public async Task<IReadOnlyList<PublisherDto>> GetTopPublishersLastYearAsync(int topCount = 5)
    {
        var publishers = await _publisherService.GetListAsync(); // 🔧 ДОБАВЛЕНО await
        var books = await _bookService.GetListAsync(); // 🔧 ДОБАВЛЕНО await
        var issues = await _issueService.GetListAsync(); // 🔧 ДОБАВЛЕНО await

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
    /// 5. Получить топ наименее популярных книг за последний год.
    /// </summary>
    public async Task<IReadOnlyList<BookDto>> GetLeastPopularBooksLastYearAsync(int topCount = 5)
    {
        var books = await _bookService.GetListAsync(); // 🔧 ДОБАВЛЕНО await
        var issues = await _issueService.GetListAsync(); // 🔧 ДОБАВЛЕНО await

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
