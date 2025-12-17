namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO для создания и обновления записи о выдаче книги.
/// Используется при создании новой выдачи и обновлении статуса возврата.
/// </summary>
public class IssueCreateUpdateDto
{
    /// <summary>
    /// Идентификатор книги, которая выдается.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Идентификатор читателя, который берет книгу.
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Дата выдачи книги читателю.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Количество дней, на которые выдается книга.
    /// </summary>
    public int DaysCount { get; set; }

    /// <summary>
    /// Фактическая дата возврата книги.
    /// Null при создании выдачи (книга ещё не возвращена).
    /// Устанавливается при возврате книги.
    /// </summary>
    public DateTime? ReturnDate { get; set; }
}
