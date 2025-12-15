using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    // GET: api/analytics/issued-books
    /// <summary>
    /// Get information about issued books ordered by title.
    /// </summary>
    [HttpGet("issued-books")]
    public async Task<ActionResult<IReadOnlyList<IssueDto>>> GetIssuedBooksOrderedByTitleAsync()
    {
        var result = await _analyticsService.GetIssuedBooksOrderedByTitleAsync();
        return Ok(result);
    }

    // GET: api/analytics/top-readers?from={from}&to={to}&topCount={topCount}
    /// <summary>
    /// Get top readers who read the most books in the given period.
    /// </summary>
    [HttpGet("top-readers")]
    public async Task<ActionResult<IReadOnlyList<ReaderDto>>> GetTopReadersByPeriodAsync(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int topCount = 5)
    {
        var result = await _analyticsService.GetTopReadersByPeriodAsync(from, to, topCount);
        return Ok(result);
    }

    // GET: api/analytics/longest-issue-period
    /// <summary>
    /// Get readers who took books for the longest period, ordered by full name.
    /// </summary>
    [HttpGet("longest-issue-period")]
    public async Task<ActionResult<IReadOnlyList<ReaderDto>>> GetReadersWithLongestIssuePeriodAsync()
    {
        var result = await _analyticsService.GetReadersWithLongestIssuePeriodAsync();
        return Ok(result);
    }

    // GET: api/analytics/top-publishers?topCount={topCount}
    /// <summary>
    /// Get top publishers for the last year.
    /// </summary>
    [HttpGet("top-publishers")]
    public async Task<ActionResult<IReadOnlyList<PublisherDto>>> GetTopPublishersLastYearAsync([FromQuery] int topCount = 5)
    {
        var result = await _analyticsService.GetTopPublishersLastYearAsync(topCount);
        return Ok(result);
    }

    // GET: api/analytics/least-popular-books?topCount={topCount}
    /// <summary>
    /// Get least popular books for the last year.
    /// </summary>
    [HttpGet("least-popular-books")]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetLeastPopularBooksLastYearAsync([FromQuery] int topCount = 5)
    {
        var result = await _analyticsService.GetLeastPopularBooksLastYearAsync(topCount);
        return Ok(result);
    }
}