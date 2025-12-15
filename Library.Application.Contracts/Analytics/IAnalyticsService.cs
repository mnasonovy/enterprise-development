using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Books;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Application service contract for analytical queries required by the lab.
/// </summary>
public interface IAnalyticsService : IApplicationService
{
    /// <summary>
    /// 1. Get information about issued books ordered by title.
    /// </summary>
    Task<IReadOnlyList<IssueDto>> GetIssuedBooksOrderedByTitleAsync();

    /// <summary>
    /// 2. Get top 5 readers who read the most books in the given period.
    /// </summary>
    Task<IReadOnlyList<ReaderDto>> GetTopReadersByPeriodAsync(DateTime from, DateTime to, int topCount = 5);

    /// <summary>
    /// 3. Get readers who took books for the longest period, ordered by full name.
    /// </summary>
    Task<IReadOnlyList<ReaderDto>> GetReadersWithLongestIssuePeriodAsync();

    /// <summary>
    /// 4. Get top 5 most popular publishers for the last year.
    /// </summary>
    Task<IReadOnlyList<PublisherDto>> GetTopPublishersLastYearAsync(int topCount = 5);

    /// <summary>
    /// 5. Get top 5 least popular books for the last year.
    /// </summary>
    Task<IReadOnlyList<BookDto>> GetLeastPopularBooksLastYearAsync(int topCount = 5);
}
