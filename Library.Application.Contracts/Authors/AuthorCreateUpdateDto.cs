using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Authors;

/// <summary>
/// DTO для создания и обновления информации об авторе.
/// Используется в POST и PUT запросах для передачи данных об авторе.
/// Содержит ID, устанавливаемый вручную при создании новой записи.
/// </summary>
public class AuthorCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор автора.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID должен быть положительным числом")]
    public int Id { get; set; }

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
