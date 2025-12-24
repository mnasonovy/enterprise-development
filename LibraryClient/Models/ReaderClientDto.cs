using Library.Application.Contracts.Issues;

namespace LibraryClient.Models;

public class ReaderClientDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<IssueDto> Issues { get; set; } = [];
}
