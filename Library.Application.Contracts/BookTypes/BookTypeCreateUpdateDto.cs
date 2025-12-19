using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO для создания и обновления типа книги.
/// Используется в POST и PUT запросах для передачи данных о типе книги.
/// Содержит ID, устанавливаемый вручную при создании новой записи.
/// </summary>
public class BookTypeCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор типа книги.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID должен быть положительным числом")]
    public int Id { get; set; }

    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// Обязательное поле, не может быть пусто, максимум 50 символов.
    /// </summary>
    [Required(ErrorMessage = "Название типа книги обязательно")]
    [StringLength(50, MinimumLength = 1,
        ErrorMessage = "Название должно быть от 1 до 50 символов")]
    public required string Name { get; set; }
}
