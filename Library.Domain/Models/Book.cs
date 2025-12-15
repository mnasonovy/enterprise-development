namespace Library.Domain.Models;

/// <summary>
/// Represents a book catalog entry in the library.
/// </summary>
public class Book
{
    /// <summary>
    /// Gets or sets the unique identifier of the book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the alphabet catalog code of the book.
    /// </summary>
    public string? AlphabetCode { get; set; }

    /// <summary>
    /// Gets or sets the title of the book.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the publication year of the book.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Foreign key to the book type.
    /// </summary>
    public int BookTypeId { get; set; }

    /// <summary>
    /// Gets or sets the book type (reference entity).
    /// </summary>
    public required BookType BookType { get; set; }

    /// <summary>
    /// Foreign key to the publisher.
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Gets or sets the publisher of the book (reference entity).
    /// </summary>
    public required Publisher Publisher { get; set; }

    /// <summary>
    /// Gets or sets the list of authors associated with the book.
    /// </summary>
    public List<Author> Authors { get; set; } = [];
}
