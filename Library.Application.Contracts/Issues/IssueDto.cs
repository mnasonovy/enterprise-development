namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO для отображения информации о выдаче книги читателю.
/// Используется в GET запросах и содержит полную информацию о выданной книге,
/// читателе, сроках выдачи и возврата.
/// </summary>
public class IssueDto
{
    /// <summary>
    /// Уникальный идентификатор записи о выдаче в базе данных.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор выданной книги.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Название выданной книги.
    /// Загружается из данных книги при получении выдачи.
    /// </summary>
    public string BookTitle { get; set; } = default!;

    /// <summary>
    /// Уникальный идентификатор читателя.
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Полное имя и фамилия читателя, который взял книгу.
    /// Загружается из данных читателя при получении выдачи.
    /// </summary>
    public string ReaderFullName { get; set; } = default!;

    /// <summary>
    /// Дата и время выдачи книги читателю.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Количество дней, на которые была выдана книга.
    /// Используется для расчета планового срока возврата.
    /// </summary>
    public int DaysCount { get; set; }

    /// <summary>
    /// Фактическая дата и время возврата книги читателем.
    /// Может быть <c>null</c> если книга еще не возвращена.
    /// Критично для отслеживания статуса выдачи и проверки просроченных книг.
    /// </summary>
    public DateTime? ReturnDate { get; set; }
}
