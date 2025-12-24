using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO для создания и обновления типа книги.
/// Используется в POST и PUT запросах для передачи данных о типе книги.
/// ID генерируется автоматически на сервере.
/// </summary>
public class BookTypeCreateUpdateDto
{
    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// Обязательное поле, не может быть пусто, максимум 50 символов.
    /// </summary>
    [Required(ErrorMessage = "Название типа книги обязательно")]
    [StringLength(50, MinimumLength = 1,
        ErrorMessage = "Название должно быть от 1 до 50 символов")]
    public required string Name { get; set; }
}
