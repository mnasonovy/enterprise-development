namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO for reading publisher information.
/// </summary>
public class PublisherDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
}
