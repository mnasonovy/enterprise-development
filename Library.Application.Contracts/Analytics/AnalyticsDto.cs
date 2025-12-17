namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Топ читателей за 6 месяцев.
/// DTO для GET /api/analytics/top-readers
/// </summary>
public class TopReaderDto
{
    /// <summary>Полное имя читателя</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Количество взятых книг</summary>
    public int CountBooks { get; set; }
}

/// <summary>
/// Статистика читателей по дням выданных книг.
/// DTO для GET /api/analytics/readers-by-days-count
/// </summary>
public class ReaderDaysCountDto
{
    /// <summary>Полное имя читателя</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Общее количество дней всех выданных книг</summary>
    public int CountDays { get; set; }
}

/// <summary>
/// Топ издательств за год.
/// DTO для GET /api/analytics/top-publishers
/// </summary>
public class TopPublisherDto
{
    /// <summary>Название издательства</summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>Количество выданных книг от этого издательства</summary>
    public int CountBooks { get; set; }
}

/// <summary>
/// Топ 5 наименее популярных книг за год.
/// DTO для GET /api/analytics/top-popular-books
/// </summary>
public class TopBookDto
{
    /// <summary>Название книги</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Количество раз, которое книга была выдана</summary>
    public int TimesIssued { get; set; }
}
