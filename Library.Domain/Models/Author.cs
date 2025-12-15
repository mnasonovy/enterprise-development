namespace Library.Domain.Models;

/// <summary>
/// Представляет автора книги с инициалами и фамилией.
/// </summary>
public class Author
{
    /// <summary>
    /// Первичный ключ.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Инициалы автора (например: "Л.Н.").
    /// Необязательное поле.
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Фамилия автора (например: "Толстой").
    /// Обязательное поле.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Навигационное свойство для книг, написанных этим автором.
    /// </summary>
    public List<Book> Books { get; set; } = [];
}
