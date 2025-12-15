namespace Library.Domain.Models;

/// <summary>
/// Represents a book author with initials and last name.
/// </summary>
public class Author
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the initials of the author (e.g., "L.N.").
    /// Optional.
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Gets or sets the last name of the author (e.g., "Tolstoy").
    /// Required.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Navigation property for books written by this author.
    /// </summary>
    public List<Book> Books { get; set; } = [];
}
