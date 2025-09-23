namespace Library.Domain.Models;

/// <summary>
/// Represents a publisher entity.
/// </summary>
public class Publisher
{
    /// <summary>
    /// Gets or sets the unique identifier of the publisher.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the publisher (e.g., "Eksmo").
    /// </summary>
    public required string Name { get; set; }
}