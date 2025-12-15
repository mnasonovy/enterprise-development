namespace Library.Domain.Models;

/// <summary>
/// Represents a type of book (e.g., Novel, Textbook).
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
    public required string Name { get; set; }

    /// <summary>
    /// Navigation property for books of this type.
    /// </summary>
    public List<Book> Books { get; set; } = [];
}
