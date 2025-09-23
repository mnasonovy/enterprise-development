namespace Library.Domain.Models;

/// <summary>
/// Represents an author of a book.
/// </summary>
public class Author
{
    /// <summary>
    /// Gets or sets the unique identifier of the author.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the initials of the author (e.g., "L.N.").
    /// </summary>
    public string Initials { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the author (e.g., "Tolstoy").
    /// </summary>
    public string LastName { get; set; } = string.Empty;
}