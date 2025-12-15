namespace Library.Domain.Models;

/// <summary>
/// Represents a library reader with personal details and registration information.
/// </summary>
public class Reader
{
    /// <summary>
    /// Gets or sets the unique identifier of the reader.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the reader.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Gets or sets the address of the reader.
    /// Optional.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the reader.
    /// Optional.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the date when the reader was registered in the library system.
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Navigation property for issues related to this reader.
    /// </summary>
    public List<Issue> Issues { get; set; } = [];
}
