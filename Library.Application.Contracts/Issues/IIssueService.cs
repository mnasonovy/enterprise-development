namespace Library.Application.Contracts.Issues;

/// <summary>
/// Application service contract for working with book issues.
/// </summary>
public interface IIssueService : IApplicationService
{
    Task<IssueDto?> GetAsync(int id);

    Task<IReadOnlyList<IssueDto>> GetListAsync();

    Task<IssueDto> CreateAsync(IssueCreateUpdateDto input);

    Task<IssueDto> UpdateAsync(int id, IssueCreateUpdateDto input);

    Task DeleteAsync(int id);
}
