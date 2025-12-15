namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO for creating or updating a reader.
/// </summary>
public class ReaderCreateUpdateDto
{
    public string FullName { get; set; } = default!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime RegistrationDate { get; set; }
}
