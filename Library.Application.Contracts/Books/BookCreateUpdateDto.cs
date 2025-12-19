using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.Books;

/// <summary>
/// DTO для создания и обновления книги.
/// Передаётся в API при создании новой книги или обновлении существующей.
/// Содержит ID, устанавливаемый вручную при создании новой записи.
/// </summary>
public class BookCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор книги.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID должен быть положительным числом")]
    public int Id { get; set; }

    /// <summary>
    /// Название книги (например: "Война и мир").
    /// Обязательное поле, не может быть пусто, максимум 255 символов.
    /// </summary>
    [Required(ErrorMessage = "Название книги обязательно")]
    [StringLength(255, MinimumLength = 1,
        ErrorMessage = "Название должно быть от 1 до 255 символов")]
    public required string Title { get; set; }

    /// <summary>
    /// Год публикации/издания книги (например: 2024, 1869).
    /// Обязательное поле, должен быть в разумном диапазоне (1000-2100).
    /// </summary>
    [Range(1000, 2100,
        ErrorMessage = "Год публикации должен быть между 1000 и 2100")]
    public int Year { get; set; }

    /// <summary>
    /// Буквенно-цифровой каталожный код книги (например: "А1", "Б2-В3").
    /// Используется для физической организации книг в библиотеке.
    /// Необязательное поле, максимум 50 символов.
    /// </summary>
    [StringLength(50,
        ErrorMessage = "Каталожный код не должен превышать 50 символов")]
    public string? AlphabetCode { get; set; }

    /// <summary>
    /// Идентификатор типа книги (роман, учебник, справочник и т.д.).
    /// Обязательное поле, должен быть > 0.
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID типа книги должен быть положительным числом")]
    public int BookTypeId { get; set; }

    /// <summary>
    /// Идентификатор издателя.
    /// Обязательное поле, должен быть > 0.
    /// </summary>
    [Range(1, int.MaxValue,
        ErrorMessage = "ID издателя должен быть положительным числом")]
    public int PublisherId { get; set; }

    /// <summary>
    /// Коллекция идентификаторов авторов, написавших эту книгу.
    /// Может быть пусто, если авторы добавляются позже.
    /// Каждый ID должен быть > 0.
    /// </summary>
    public List<int> AuthorIds { get; set; } = [];
}
