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
