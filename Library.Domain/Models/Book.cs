namespace Library.Domain.Models;

/// <summary>
/// Представляет запись о книге в каталоге библиотеки.
/// </summary>
public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Буквенный код каталога книги (например: "А1", "Б2").
    /// Опциональное поле.
    /// </summary>
    public string? AlphabetCode { get; set; }

    /// <summary>
    /// Название книги.
    /// Обязательное поле.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Год публикации книги.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Внешний ключ на тип книги.
    /// </summary>
    public int BookTypeId { get; set; }

    /// <summary>
    /// Тип книги (справочная сущность: роман, учебник и т.д.).
    /// Обязательное поле.
    /// </summary>
    public required BookType BookType { get; set; }

    /// <summary>
    /// Внешний ключ на издателя.
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Издатель книги (справочная сущность).
    /// Обязательное поле.
    /// </summary>
    public required Publisher Publisher { get; set; }

    /// <summary>
    /// Список авторов, связанных с этой книгой.
    /// Отношение "много-ко-многим".
    /// </summary>
    public List<Author> Authors { get; set; } = [];

    /// <summary>
    /// Навигационное свойство - обратная связь к Issue для корректной работы EF Core.
    /// Необходимо для предотвращения ошибок маппинга при загрузке связанных данных через Include().
    /// </summary>
    public List<Issue> Issues { get; set; } = [];
}
