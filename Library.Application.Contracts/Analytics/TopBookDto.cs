using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Analytics;

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
