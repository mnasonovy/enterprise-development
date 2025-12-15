namespace Library.Domain.Models;

/// <summary>
/// Represents a record of a book issued to a reader.
/// </summary>
public class Issue
{
    /// <summary>
    /// Gets or sets the unique identifier of the issue record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the issued book.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Gets or sets the book that was issued.
    /// </summary>
    public required Book Book { get; set; }

    /// <summary>
    /// Foreign key to the reader.
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Gets or sets the reader who took the book.
    /// </summary>
    public required Reader Reader { get; set; }

    /// <summary>
    /// Gets or sets the date when the book was issued.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the number of days the book was issued for.
    /// </summary>
    public int DaysCount { get; set; }
}
