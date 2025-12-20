using Library.Application.Contracts.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения аналитических запросов по библиотеке.
/// Предоставляет REST API endpoints для получения полной информации об аналитике:
/// - Список выданных книг в алфавитном порядке
/// - Топ читателей за последние 6 месяцев
/// - Распределение читателей по дням выдачи книг
/// - Топ издательств за последний год
/// - Рейтинг наименее популярных книг за год
/// 
/// Все методы доступны только для чтения (GET запросы).
/// Возвращает результаты в формате JSON с полной информацией для отчетности и анализа.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить все выданные книги в библиотеке в алфавитном порядке.
    /// GET: /api/analytics/issued-books-titles
    /// </summary>
    [HttpGet("issued-books-titles")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<string>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetIssuedBooksOrderedByTitleAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetIssuedBooksOrderedByTitleAsync));
        var result = await analyticsService.GetIssuedBooksOrderedByTitleAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить топ 5 читателей по количеству взятых книг за последние 6 месяцев.
    /// GET: /api/analytics/top-readers
    /// </summary>
    [HttpGet("top-readers")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<TopReaderDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopReadersAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetTopReadersAsync));
        var result = await analyticsService.GetTopReadersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить статистику читателей по общей длительности дней выданных книг.
    /// GET: /api/analytics/readers-by-days-count
    /// </summary>
    [HttpGet("readers-by-days-count")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<ReaderDaysCountDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetReadersByDaysCountAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetReadersByDaysCountAsync));
        var result = await analyticsService.GetReadersByDaysCountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить топ 5 издательств по количеству выданных книг за последний год.
    /// GET: /api/analytics/top-publishers
    /// </summary>
    [HttpGet("top-publishers")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<TopPublisherDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopPublishersLastYearAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetTopPublishersLastYearAsync));
        var result = await analyticsService.GetTopPublishersLastYearAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить топ 5 популярных книг за последний год по количеству выданий.
    /// GET: /api/analytics/top-popular-books
    /// </summary>
    [HttpGet("top-popular-books")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<TopBookDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopPopularBooksLastYearAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetTopPopularBooksLastYearAsync));
        var result = await analyticsService.GetTopPopularBooksLastYearAsync();
        return Ok(result);
    }
}
