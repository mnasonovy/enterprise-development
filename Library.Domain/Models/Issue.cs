namespace Library.Domain.Models;

/// <summary>
/// Represents a record of a book issue (when a reader borrows a book from the library).
/// </summary>
public class Issue
{
    /// <summary>
    /// Gets or sets the unique identifier of the issue record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the issued book (reference to <see cref="Book"/>).
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the reader who borrowed the book (reference to <see cref="Reader"/>).
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Gets or sets the date when the book was issued.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the number of days for which the book was issued.
    /// </summary>
    public int DaysCount { get; set; }
}