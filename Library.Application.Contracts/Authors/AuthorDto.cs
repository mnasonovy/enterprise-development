namespace Library.Application.Contracts.Authors;

/// <summary>
/// DTO for reading author information.
/// </summary>
public class AuthorDto
{
    public int Id { get; set; }

    public string? Initials { get; set; }

    public string LastName { get; set; } = default!;
}
