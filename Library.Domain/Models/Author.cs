namespace Library.Domain.Models;

/// <summary>
/// Представляет автора книги в системе библиотеки.
/// Содержит основную информацию об авторе: инициалы и фамилию.
/// </summary>
public class Author
{
    /// <summary>
    /// Уникальный идентификатор автора в базе данных.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Инициалы автора (например: "Л.Н.", "Ф.М.").
    /// Необязательное поле.
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Фамилия автора (например: "Толстой", "Достоевский").
    /// Обязательное поле.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Коллекция книг, написанных этим автором.
    /// Связь один-ко-многим с таблицей Books.
    /// </summary>
    public List<Book> Books { get; set; } = [];
}
