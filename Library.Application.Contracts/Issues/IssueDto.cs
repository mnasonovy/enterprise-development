namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO для чтения информации о выдаче книги.
/// Содержит все данные о выданной книге и дате её возврата.
/// </summary>
public class IssueDto
{
    /// <summary>
    /// Уникальный идентификатор выдачи.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор выданной книги.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Название выданной книги.
    /// </summary>
    public string BookTitle { get; set; } = default!;

    /// <summary>
    /// Идентификатор читателя.
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Полное имя читателя.
    /// </summary>
    public string ReaderFullName { get; set; } = default!;

    /// <summary>
    /// Дата выдачи книги.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Количество дней, на которые выдана книга.
    /// </summary>
    public int DaysCount { get; set; }

    /// <summary>
    /// Фактическая дата возврата книги.
    /// Null, если книга ещё не возвращена.
    /// Критично для отслеживания статуса выдачи и просроченных книг.
    /// </summary>
    public DateTime? ReturnDate { get; set; }
}
