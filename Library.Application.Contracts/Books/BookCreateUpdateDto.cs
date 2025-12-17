namespace Library.Application.Contracts.Books;

/// <summary>
/// DTO для создания и обновления книги.
/// Передаётся в API при создании новой книги или обновлении существующей.
/// </summary>
public class BookCreateUpdateDto
{
    /// <summary>
    /// Название книги (например: "Война и мир").
    /// Обязательное поле.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Год публикации/издания книги (например: 2024).
    /// Обязательное поле.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Буквенно-цифровой каталожный код книги (например: "А1", "Б2").
    /// Используется для физической организации книг в библиотеке.
    /// Необязательное поле.
    /// </summary>
    public string? AlphabetCode { get; set; }

    /// <summary>
    /// Идентификатор типа книги (роман, учебник, справочник и т.д.).
    /// Обязательное поле.
    /// </summary>
    public int BookTypeId { get; set; }

    /// <summary>
    /// Идентификатор издателя.
    /// Обязательное поле.
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Коллекция идентификаторов авторов, написавших эту книгу.
    /// Может быть пусто, если авторы добавляются позже.
    /// </summary>
    public List<int> AuthorIds { get; set; } = [];
}
