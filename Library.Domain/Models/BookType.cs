namespace Library.Domain.Models;

/// <summary>
/// Represents the type of a book (e.g., novel, textbook, magazine).
/// </summary>
public class BookType
{
    /// <summary>
    /// Gets or sets the unique identifier of the book type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the book type (e.g., "Novel", "Textbook").
    /// </summary>
    public string Name { get; set; } = string.Empty;
}