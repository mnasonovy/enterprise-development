namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO for reading reader information.
/// </summary>
public class ReaderDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = default!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime RegistrationDate { get; set; }
}
