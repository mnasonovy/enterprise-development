namespace Library.Domain.Models;

/// <summary>
/// Represents a catalog entry of a book in the library.
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
    public string AlphabetCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title of the book.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the year of publication of the book.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the book type (reference to <see cref="BookType"/>).
    /// </summary>
    public int BookTypeId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the publisher (reference to <see cref="Publisher"/>).
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Gets or sets the identifiers of the authors associated with this book.
    /// One book can have multiple authors.
    /// </summary>
    public List<int> AuthorIds { get; set; } = new();
}