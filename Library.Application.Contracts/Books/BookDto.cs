namespace Library.Application.Contracts.Books;

/// <summary>
/// Data transfer object for reading book information.
/// </summary>
public class BookDto
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public int Year { get; set; }

    public string? AlphabetCode { get; set; }

    public string BookTypeName { get; set; } = default!;

    public string PublisherName { get; set; } = default!;

    public List<string> AuthorNames { get; set; } = [];
}
