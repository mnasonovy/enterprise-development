namespace Library.Domain.Models;

/// <summary>
/// Представляет запись о выдаче книги читателю.
/// </summary>
public class Issue
{
    /// <summary>
    /// Уникальный идентификатор записи о выдаче.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Внешний ключ на выданную книгу.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Книга, которая была выдана читателю.
    /// Обязательное поле.
    /// </summary>
    public required Book Book { get; set; }

    /// <summary>
    /// Внешний ключ на читателя.
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Читатель, который взял книгу.
    /// Обязательное поле.
    /// </summary>
    public required Reader Reader { get; set; }

    /// <summary>
    /// Дата выдачи книги читателю.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Количество дней, на которые выдана книга.
    /// </summary>
    public int DaysCount { get; set; }

    /// <summary>
    /// Фактическая дата возврата книги.
    /// Null, если книга еще не возвращена.
    /// Используется для отслеживания статуса выдачи и проверки просроченных книг.
    /// </summary>
    public DateTime? ReturnDate { get; set; }
}
