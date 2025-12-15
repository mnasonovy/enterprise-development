namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO for reading book type information.
/// </summary>
public class BookTypeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
}
