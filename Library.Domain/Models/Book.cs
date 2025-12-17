namespace Library.Domain.Models;

/// <summary>
/// Представляет книгу в каталоге библиотеки.
/// Содержит основную информацию о книге, включая название, год публикации, тип и издателя.
/// </summary>
public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги в базе данных.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Буквенно-цифровой каталожный код книги (например: "А1", "Б2").
    /// Используется для физической организации книг в библиотеке.
    /// Необязательное поле.
    /// </summary>
    public string? AlphabetCode { get; set; }

    /// <summary>
    /// Название книги (например: "Война и мир").
    /// Обязательное поле.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Год публикации/издания книги (например: 2024).
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Уникальный идентификатор типа книги.
    /// Используется как внешний ключ для связи с таблицей BookType.
    /// </summary>
    public int BookTypeId { get; set; }

    /// <summary>
    /// Тип книги (роман, учебник, справочник и т.д.).
    /// Обязательное поле. Представляет справочную сущность.
    /// </summary>
    public required BookType BookType { get; set; }

    /// <summary>
    /// Уникальный идентификатор издателя.
    /// Используется как внешний ключ для связи с таблицей Publisher.
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Издатель книги.
    /// Обязательное поле. Представляет справочную сущность.
    /// </summary>
    public required Publisher Publisher { get; set; }

    /// <summary>
    /// Коллекция авторов, написавших эту книгу.
    /// Отношение "многие-ко-многим" (Book ↔ Author).
    /// </summary>
    public List<Author> Authors { get; set; } = [];

    /// <summary>
    /// Коллекция выпусков (экземпляров) этой книги в библиотеке.
    /// Обратная навигационная ссылка на сущность Issue для корректной работы Entity Framework Core.
    /// </summary>
    public List<Issue> Issues { get; set; } = [];
}
