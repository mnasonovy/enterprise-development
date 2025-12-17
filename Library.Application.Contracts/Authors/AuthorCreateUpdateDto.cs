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
    public int Id { get; set; }

    /// <summary>
    /// Инициалы автора (например: "Л.Н.", "Ф.М.").
    /// Необязательное поле.
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Фамилия автора (например: "Толстой", "Достоевский").
    /// Обязательное поле, не может быть пусто.
    /// </summary>
    public required string LastName { get; set; }
}
