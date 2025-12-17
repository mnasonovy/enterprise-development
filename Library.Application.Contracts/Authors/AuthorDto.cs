namespace Library.Application.Contracts.Authors;

/// <summary>
/// DTO для передачи данных об авторе между слоями приложения.
/// Используется при получении, создании и обновлении информации об авторе.
/// </summary>
public class AuthorDto
{
    /// <summary>
    /// Уникальный идентификатор автора.
    /// При создании автора это поле не передаётся (генерируется БД).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Инициалы автора (например: "Л.Н.", "Ф.М.").
    /// Необязательное поле.
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Фамилия автора (например: "Толстой", "Достоевский").
    /// Обязательное поле при создании и обновлении.
    /// </summary>
    public required string LastName { get; set; }
}
