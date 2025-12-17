namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Интерфейс для аналитического сервиса библиотеки.
/// Определяет методы для получения различных аналитических отчетов.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получить все выданные книги в алфавитном порядке (уникальные названия).
    /// </summary>
    public Task<IReadOnlyList<string>> GetIssuedBooksOrderedByTitleAsync();

    /// <summary>
    /// Получить топ 5 читателей по количеству взятых книг за последние 6 месяцев.
    /// </summary>
    public Task<IReadOnlyList<TopReaderDto>> GetTopReadersAsync();

    /// <summary>
    /// Получить всех читателей с общим количеством дней выданных книг, отсортированных по имени.
    /// </summary>
    public Task<IReadOnlyList<ReaderDaysCountDto>> GetReadersByDaysCountAsync();

    /// <summary>
    /// Получить топ 5 издательств по количеству выданных книг за последний год.
    /// </summary>
    public Task<IReadOnlyList<TopPublisherDto>> GetTopPublishersLastYearAsync();

    /// <summary>
    /// Получить топ 5 наименее популярных книг за последний год.
    /// </summary>
    public Task<IReadOnlyList<TopBookDto>> GetTopPopularBooksLastYearAsync();
}
