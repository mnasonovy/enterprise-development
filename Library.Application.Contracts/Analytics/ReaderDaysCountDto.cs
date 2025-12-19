using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Analytics;

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
