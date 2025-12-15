using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Publishers;

namespace Library.Application.Services;

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
    /// 1. Get information about issued books ordered by title.
    /// </summary>
    public async Task<IReadOnlyList<IssueDto>> GetIssuedBooksOrderedByTitleAsync()
    {
        var issues = await _issueService.GetListAsync();

        var issuedBooks = issues
            .OrderBy(i => i.BookTitle)
            .ToList();

        return issuedBooks;
    }

    /// <summary>
    /// 2. Get top 5 readers who read the most books in the given period.
    /// </summary>
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
            .ToList();

        return topReaders!;
    }

    /// <summary>
    /// 3. Get readers who took books for the longest period, ordered by full name.
    /// </summary>
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
            .ToList();

        return readersWithLongestPeriod!;
    }

    /// <summary>
    /// 4. Get top 5 most popular publishers for the last year.
    /// </summary>
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
            .ToList();

        return topPublishers!;
    }

    /// <summary>
    /// 5. Get top 5 least popular books for the last year.
    /// </summary>
    public async Task<IReadOnlyList<BookDto>> GetLeastPopularBooksLastYearAsync(int topCount = 5)
    {
        var books = await _bookService.GetListAsync();
        var issues = await _issueService.GetListAsync();

        var lastYear = DateTime.Now.AddYears(-1);

        var leastPopularBooks = books
            .GroupBy(b => b.Id)
            .Select(g => new
            {
                Book = g.First(),
                IssueCount = issues
                    .Where(i => i.BookId == g.Key && i.IssueDate >= lastYear)
                    .Count()
            })
            .OrderBy(x => x.IssueCount)
            .Take(topCount)
            .Select(x => x.Book)
            .ToList();

        return leastPopularBooks;
    }
}