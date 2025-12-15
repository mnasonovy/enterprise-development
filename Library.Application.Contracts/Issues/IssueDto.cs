namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO for reading issue (book lending) information.
/// </summary>
public class IssueDto
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string BookTitle { get; set; } = default!;

    public int ReaderId { get; set; }

    public string ReaderFullName { get; set; } = default!;

    public DateTime IssueDate { get; set; }

    public int DaysCount { get; set; }
}
