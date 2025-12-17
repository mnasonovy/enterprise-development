using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Books;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Интерфейс сервиса для аналитических запросов по библиотеке.
/// Определяет контракт для выполнения сложных аналитических операций с данными о выданных книгах.
/// </summary>
public interface IAnalyticsService : IApplicationService
{
    /// <summary>
    /// Получить информацию о всех выданных книгах, упорядоченных по названию.
    /// </summary>
    /// <returns>Список всех выданных книг отсортированный по названию</returns>
    public Task<IReadOnlyList<IssueDto>> GetIssuedBooksOrderedByTitleAsync();

    /// <summary>
    /// Получить топ N читателей, которые прочитали больше всего книг за заданный период.
    /// </summary>
    /// <param name="from">Начало периода поиска</param>
    /// <param name="to">Конец периода поиска</param>
    /// <param name="topCount">Количество читателей в топе (по умолчанию 5)</param>
    /// <returns>Список DTO читателей с количеством книг в периоде</returns>
    public Task<IReadOnlyList<ReaderDto>> GetTopReadersByPeriodAsync(DateTime from, DateTime to, int topCount = 5);

    /// <summary>
    /// Получить читателей, которые брали книги на самый длительный период, упорядоченных по полному имени.
    /// </summary>
    /// <returns>Список DTO читателей упорядоченный по полному имени</returns>
    public Task<IReadOnlyList<ReaderDto>> GetReadersWithLongestIssuePeriodAsync();

    /// <summary>
    /// Получить топ N наиболее популярных издателей за последний год.
    /// </summary>
    /// <param name="topCount">Количество издателей в топе (по умолчанию 5)</param>
    /// <returns>Список DTO издателей упорядоченный по популярности</returns>
    public Task<IReadOnlyList<PublisherDto>> GetTopPublishersLastYearAsync(int topCount = 5);

    /// <summary>
    /// Получить топ N наименее популярных книг за последний год.
    /// </summary>
    /// <param name="topCount">Количество книг в топе (по умолчанию 5)</param>
    /// <returns>Список DTO книг упорядоченный по популярности в возрастающем порядке</returns>
    public Task<IReadOnlyList<BookDto>> GetLeastPopularBooksLastYearAsync(int topCount = 5);
}
