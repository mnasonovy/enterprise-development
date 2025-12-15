namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO for creating or updating a publisher.
/// </summary>
public class PublisherCreateUpdateDto
{
    public string Name { get; set; } = default!;
}
