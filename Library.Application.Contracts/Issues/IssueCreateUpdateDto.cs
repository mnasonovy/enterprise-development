using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO для создания и обновления выданной книги.
/// Используется в POST (создание) и PUT (обновление/возврат) запросах.
/// </summary>
public class IssueCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор выдачи.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID должен быть положительным числом")]
    public int Id { get; set; }

    /// <summary>
    /// ID книги, которая выдается.
    /// Обязательное поле, должен быть > 0.
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID книги должен быть положительным числом")]
    public int BookId { get; set; }

    /// <summary>
    /// ID читателя, который берет книгу.
    /// Обязательное поле, должен быть > 0.
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID читателя должен быть положительным числом")]
    public int ReaderId { get; set; }

    /// <summary>
    /// Дата выдачи книги читателю (UTC).
    /// Обязательное поле, должна быть в разумном диапазоне (1900-2100).
    /// </summary>
    [Range(typeof(DateTime), "1900-01-01", "2100-12-31",
        ErrorMessage = "Дата выдачи должна быть между 1900 и 2100 годами")]
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Количество дней выдачи книги (например: 14).
    /// Обязательное поле, должно быть в диапазоне 1-30 дней.
    /// </summary>
    [Range(1, 30,
        ErrorMessage = "Количество дней должно быть от 1 до 30")]
    public int DaysCount { get; set; }

    /// <summary>
    /// Дата возврата книги читателем (UTC).
    /// Необязательное поле, null если книга еще не возвращена.
    /// Если указана, должна быть в разумном диапазоне (1900-2100).
    /// </summary>
    [Range(typeof(DateTime), "1900-01-01", "2100-12-31",
        ErrorMessage = "Дата возврата должна быть между 1900 и 2100 годами")]
    public DateTime? ReturnDate { get; set; }
}
