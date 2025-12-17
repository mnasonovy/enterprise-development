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
    /// 
    /// Методология:
    /// - Собирает все записи о выданных книгах
    /// - Извлекает уникальные названия книг (исключает дубликаты)
    /// - Сортирует результат в алфавитном порядке от A до Z
    /// 
    /// Использование:
    /// - Для составления полного каталога выданной литературы
    /// - Для анализа разнообразия книг в обороте библиотеки
    /// - Для проверки полноты каталога выданных издаций
    /// 
    /// GET: /api/analytics/issued-books-titles
    /// </summary>
    /// <returns>Список уникальных названий книг, отсортированный в алфавитном порядке (A-Z)</returns>
    [HttpGet("issued-books-titles")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<string>))]
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
    /// Получить топ 5 читателей по количеству взятых книг за последние 6 месяцев.
    /// 
    /// Методология:
    /// - Анализирует все выданные книги за последние 6 месяцев
    /// - Группирует по читателям и подсчитывает количество выданных книг
    /// - Выбирает 5 читателей с максимальным числом выданных экземпляров
    /// - Сортирует в порядке убывания по количеству книг
    /// 
    /// Использование:
    /// - Для выявления самых активных читателей
    /// - Для поощрения и мотивации постоянных читателей
    /// - Для анализа читательской активности за последний период
    /// 
    /// Возвращает:
    /// - FullName: Полное имя читателя
    /// - CountBooks: Количество взятых книг за 6 месяцев
    /// 
    /// GET: /api/analytics/top-readers
    /// </summary>
    /// <returns>Список топ 5 читателей с именем и количеством взятых книг, отсортированный по убыванию активности</returns>
    [HttpGet("top-readers")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<TopReaderDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopReadersAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetTopReadersAsync));

        var result = await analyticsService.GetTopReadersAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopReadersAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить статистику читателей по общей длительности дней выданных книг.
    /// 
    /// Методология:
    /// - Агрегирует все выданные книги для каждого читателя
    /// - Суммирует количество дней для каждой выданной книги
    /// - Вычисляет общее количество дней для каждого читателя
    /// - Сортирует читателей по алфавиту по полному имени
    /// 
    /// Использование:
    /// - Для анализа средней длительности использования книг читателями
    /// - Для выявления читателей, которые дольше хранят книги
    /// - Для оптимизации политики выдачи книг
    /// - Для анализа привычек читателей по периоду удержания литературы
    /// 
    /// Возвращает:
    /// - FullName: Полное имя читателя
    /// - CountDays: Суммарное количество дней для всех выданных этому читателю книг
    /// 
    /// GET: /api/analytics/readers-by-days-count
    /// </summary>
    /// <returns>Список всех читателей с общим количеством дней выданных книг, отсортированный по алфавиту</returns>
    [HttpGet("readers-by-days-count")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<ReaderDaysCountDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetReadersByDaysCountAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetReadersByDaysCountAsync));

        var result = await analyticsService.GetReadersByDaysCountAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetReadersByDaysCountAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить топ 5 издательств по количеству выданных книг за последний год.
    /// 
    /// Методология:
    /// - Фильтрует все выданные книги за последний год (365 дней)
    /// - Группирует по издательствам через связь книга → издатель
    /// - Суммирует количество выданных экземпляров по каждому издательству
    /// - Выбирает топ 5 издательств с наибольшим числом выданий
    /// - Сортирует в порядке убывания
    /// 
    /// Использование:
    /// - Для выявления партнеров-издательств с наиболее востребованной литературой
    /// - Для анализа популярности издательств в библиотеке
    /// - Для планирования закупок и развития парка литературы
    /// - Для составления рейтинга издательств по спросу читателей
    /// 
    /// Возвращает:
    /// - PublisherName: Название издательства
    /// - CountBooks: Количество выданных экземпляров за год
    /// 
    /// GET: /api/analytics/top-publishers
    /// </summary>
    /// <returns>Список топ 5 издательств с названием и общим количеством выданных книг, отсортированный по популярности</returns>
    [HttpGet("top-publishers")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<TopPublisherDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopPublishersLastYearAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetTopPublishersLastYearAsync));

        var result = await analyticsService.GetTopPublishersLastYearAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopPublishersLastYearAsync), result.Count);

        return Ok(result);
    }

    /// <summary>
    /// Получить топ 5 наименее популярных книг за последний год по количеству выданий.
    /// 
    /// Методология:
    /// - Фильтрует все выданные книги за последний год (365 дней)
    /// - Группирует по книгам и подсчитывает количество выданий
    /// - Сортирует в порядке возрастания (меньше всего выданных)
    /// - Выбирает 5 книг с наименьшим количеством выданий
    /// 
    /// Использование:
    /// - Для выявления невостребованной литературы в фондах
    /// - Для принятия решений о переводе книг в хранилище
    /// - Для анализа соответствия фонда интересам читателей
    /// - Для оптимизации состава библиотечного парка
    /// - Для выявления книг, требующих переоценки или переиздания
    /// 
    /// Возвращает:
    /// - Title: Название книги
    /// - TimesIssued: Количество раз, которая книга была выдана (минимум за год)
    /// 
    /// GET: /api/analytics/top-popular-books
    /// </summary>
    /// <returns>Список топ 5 наименее популярных книг с названием и количеством выданий, отсортированный по возрастанию</returns>
    [HttpGet("top-popular-books")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<TopBookDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTopPopularBooksLastYearAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetTopPopularBooksLastYearAsync));

        var result = await analyticsService.GetTopPopularBooksLastYearAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetTopPopularBooksLastYearAsync), result.Count);

        return Ok(result);
    }
}
