using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Analytics;

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
