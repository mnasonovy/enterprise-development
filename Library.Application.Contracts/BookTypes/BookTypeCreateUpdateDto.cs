namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO for creating or updating a book type.
/// </summary>
public class BookTypeCreateUpdateDto
{
    public string Name { get; set; } = default!;
}
