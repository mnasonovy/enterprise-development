namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO for creating or updating an issue record.
/// </summary>
public class IssueCreateUpdateDto
{
    public int BookId { get; set; }

    public int ReaderId { get; set; }

    public DateTime IssueDate { get; set; }

    public int DaysCount { get; set; }
}