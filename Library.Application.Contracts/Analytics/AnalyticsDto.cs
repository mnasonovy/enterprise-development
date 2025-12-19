using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Топ читателей за 6 месяцев.
/// DTO для GET /api/analytics/top-readers
/// </summary>
public class TopReaderDto
{
    [Required(ErrorMessage = "Имя читателя обязательно")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Имя должно быть от 2 до 200 символов")]
    public string FullName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Количество книг должно быть > 0")]
    public int CountBooks { get; set; }
}

/// <summary>
/// Статистика читателей по дням выданных книг.
/// DTO для GET /api/analytics/readers-by-days-count
/// </summary>
public class ReaderDaysCountDto
{
    [Required(ErrorMessage = "Имя читателя обязательно")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Имя должно быть от 2 до 200 символов")]
    public string FullName { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Количество дней не может быть отрицательным")]
    public int CountDays { get; set; }
}

/// <summary>
/// Топ издательств за год.
/// DTO для GET /api/analytics/top-publishers
/// </summary>
public class TopPublisherDto
{
    [Required(ErrorMessage = "Название издательства обязательно")]
    [StringLength(300, MinimumLength = 2,
        ErrorMessage = "Название должно быть от 2 до 300 символов")]
    public string PublisherName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Количество книг должно быть > 0")]
    public int CountBooks { get; set; }
}

/// <summary>
/// Топ 5 популярных книг за год.
/// DTO для GET /api/analytics/top-popular-books
/// </summary>
public class TopBookDto
{
    [Required(ErrorMessage = "Название книги обязательно")]
    [StringLength(500, MinimumLength = 1,
        ErrorMessage = "Название должно быть от 1 до 500 символов")]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Количество выданных копий должно быть > 0")]
    public int TimesIssued { get; set; }
}
