using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Authors;

/// <summary>
/// DTO для создания и обновления информации об авторе.
/// Используется в POST и PUT запросах для передачи данных об авторе.
/// ID генерируется автоматически на сервере.
/// </summary>
public class AuthorCreateUpdateDto
{
    /// <summary>
    /// Инициалы автора (например: "Л.Н.", "Ф.М.").
    /// Необязательное поле, максимум 10 символов.
    /// </summary>
    [StringLength(10,
        ErrorMessage = "Инициалы не должны превышать 10 символов")]
    public string? Initials { get; set; }

    /// <summary>
    /// Фамилия автора (например: "Толстой", "Достоевский").
    /// Обязательное поле, не может быть пусто, максимум 100 символов.
    /// </summary>
    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(100, MinimumLength = 1,
        ErrorMessage = "Фамилия должна быть от 1 до 100 символов")]
    public required string LastName { get; set; }
}
