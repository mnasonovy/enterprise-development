namespace Library.Application.Contracts.Authors;

/// <summary>
/// DTO for creating or updating an author.
/// </summary>
public class AuthorCreateUpdateDto
{
    public string? Initials { get; set; }

    public string LastName { get; set; } = default!;
}
