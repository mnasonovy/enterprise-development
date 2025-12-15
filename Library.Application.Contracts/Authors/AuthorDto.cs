namespace Library.Application.Contracts.Authors;

/// <summary>
/// Data Transfer Object для передачи данных автора между слоями приложения
/// Используется для создания, обновления и возврата информации об авторе
/// </summary>
public class AuthorDto
{
    /// <summary>
    /// Уникальный идентификатор автора
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Инициалы автора (например: "Л.Н.", "Ф.М.")
    /// Может быть null если инициалы не указаны
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Фамилия автора
    /// Обязательное поле
    /// </summary>
    public required string LastName { get; set; }
}
