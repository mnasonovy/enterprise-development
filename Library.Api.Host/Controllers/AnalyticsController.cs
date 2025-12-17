using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения аналитических запросов по библиотеке.
/// Предоставляет REST API endpoints для получения аналитических данных (только GET методы).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить информацию о выданных книгах, упорядоченные по названию.
    /// GET: /api/analytics/issued-books
    /// </summary>
    /// <returns>Список всех выданных книг отсортированный по названию</returns>
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
    /// Получить топ N читателей, которые взяли больше всего книг в заданный период.
    /// GET: /api/analytics/top-readers?from={from}&to={to}&topCount={topCount}
    /// </summary>
    /// <param name="from">Начало периода поиска</param>
    /// <param name="to">Конец периода поиска</param>
    /// <param name="topCount">Количество читателей в топе (по умолчанию 5)</param>
    /// <returns>Список DTO читателей с наибольшим количеством выданных книг в периоде</returns>
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
    /// Получить читателей, которые брали книги на самый длительный период, упорядоченных по полному имени.
    /// GET: /api/analytics/longest-issue-period
    /// </summary>
    /// <returns>Список DTO читателей упорядоченный по полному имени с максимальным средним периодом выдачи</returns>
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
    /// Получить топ N наиболее популярных издателей за последний год.
    /// GET: /api/analytics/top-publishers?topCount={topCount}
    /// </summary>
    /// <param name="topCount">Количество издателей в топе (по умолчанию 5)</param>
    /// <returns>Список DTO издателей упорядоченный по количеству выданных книг в убывающем порядке</returns>
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
    /// Получить топ N наименее популярных книг за последний год.
    /// GET: /api/analytics/least-popular-books?topCount={topCount}
    /// </summary>
    /// <param name="topCount">Количество книг в топе (по умолчанию 5)</param>
    /// <returns>Список DTO книг упорядоченный по количеству выданных копий в возрастающем порядке</returns>
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
