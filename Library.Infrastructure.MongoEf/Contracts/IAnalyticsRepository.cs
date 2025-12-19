using Library.Application.Contracts.Analytics;

namespace Library.Infrastructure.MongoEf.Contracts;

/// <summary>
/// Репозиторий для аналитических запросов.
/// Работает напрямую с DbSet без загрузки в память всех данных.
/// </summary>
public interface IAnalyticsRepository
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
    /// Получить всех читателей со статистикой по дням выданных книг.
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
