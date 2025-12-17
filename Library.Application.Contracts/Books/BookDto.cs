namespace Library.Application.Contracts.Books;

/// <summary>
/// DTO для передачи данных о книге между слоями приложения.
/// Используется при получении информации о книге с полными данными типа и издателя.
/// </summary>
public class BookDto
{
    /// <summary>
    /// Уникальный идентификатор книги.
    /// При создании книги это поле не передаётся (генерируется БД).
    /// </summary>
    public int Id { get; set; }

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
    /// Буквенно-цифровой каталожный код книги (например: "А1", "Б2").
    /// Используется для физической организации книг в библиотеке.
    /// Необязательное поле.
    /// </summary>
    public string? AlphabetCode { get; set; }

    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// Заполняется автоматически при преобразовании из Domain модели.
    /// </summary>
    public required string BookTypeName { get; set; }

    /// <summary>
    /// Название издателя книги.
    /// Заполняется автоматически при преобразовании из Domain модели.
    /// </summary>
    public required string PublisherName { get; set; }

    /// <summary>
    /// Коллекция имён авторов (инициалы + фамилия).
    /// Может быть пусто, если авторы не добавлены.
    /// </summary>
    public List<string> AuthorNames { get; set; } = [];
}
