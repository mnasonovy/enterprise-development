namespace Library.Application.Contracts.Books;

/// <summary>
/// DTO for creating or updating a book.
/// </summary>
public class BookCreateUpdateDto
{
    public string Title { get; set; } = default!;

    public int Year { get; set; }

    public string? AlphabetCode { get; set; }

    public int BookTypeId { get; set; }

    public int PublisherId { get; set; }

    public List<int> AuthorIds { get; set; } = [];
}
