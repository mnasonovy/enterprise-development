using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для аналитических запросов (только GET методы с query параметрами)
/// 🔧 ИСПРАВЛЕНО: Primary Constructor, логирование, валидация, документация
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить информацию о выданных книгах, упорядоченные по названию
    /// GET: /api/analytics/issued-books
    /// </summary>
    [HttpGet("issued-books")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<IssueDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetIssuedBooksOrderedByTitleAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetIssuedBooksOrderedByTitleAsync));

        var result = await analyticsService.GetIssuedBooksOrderedByTitleAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetIssuedBooksOrderedByTitleAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить топ читателей, которые взяли больше всего книг в период
    /// GET: /api/analytics/top-readers?from={from}&to={to}&topCount={topCount}
    /// </summary>
    [HttpGet("top-readers")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<ReaderDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopReadersByPeriodAsync(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int topCount = 5)
    {
        logger.LogInformation("{Method} method is called with from={From}, to={To}, topCount={TopCount}",
            nameof(GetTopReadersByPeriodAsync), from, to, topCount);

        if (from == default || to == default)
            return BadRequest("Both 'from' and 'to' dates are required");

        if (from > to)
            return BadRequest("'from' date must be less than or equal to 'to' date");

        if (topCount <= 0)
            return BadRequest("topCount must be greater than 0");

        var result = await analyticsService.GetTopReadersByPeriodAsync(from, to, topCount);

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopReadersByPeriodAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить читателей, которые брали книги на самый длинный период
    /// GET: /api/analytics/longest-issue-period
    /// </summary>
    [HttpGet("longest-issue-period")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<ReaderDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetReadersWithLongestIssuePeriodAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetReadersWithLongestIssuePeriodAsync));

        var result = await analyticsService.GetReadersWithLongestIssuePeriodAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetReadersWithLongestIssuePeriodAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить топ издателей за последний год
    /// GET: /api/analytics/top-publishers?topCount={topCount}
    /// </summary>
    [HttpGet("top-publishers")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<PublisherDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopPublishersLastYearAsync([FromQuery] int topCount = 5)
    {
        logger.LogInformation("{Method} method is called with topCount={TopCount}",
            nameof(GetTopPublishersLastYearAsync), topCount);

        if (topCount <= 0)
            return BadRequest("topCount must be greater than 0");

        var result = await analyticsService.GetTopPublishersLastYearAsync(topCount);

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopPublishersLastYearAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить топ наименее популярных книг за последний год
    /// GET: /api/analytics/least-popular-books?topCount={topCount}
    /// </summary>
    [HttpGet("least-popular-books")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetLeastPopularBooksLastYearAsync([FromQuery] int topCount = 5)
    {
        logger.LogInformation("{Method} method is called with topCount={TopCount}",
            nameof(GetLeastPopularBooksLastYearAsync), topCount);

        if (topCount <= 0)
            return BadRequest("topCount must be greater than 0");

        var result = await analyticsService.GetLeastPopularBooksLastYearAsync(topCount);

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetLeastPopularBooksLastYearAsync), result.Count);

        return Ok(result);
    }
}
