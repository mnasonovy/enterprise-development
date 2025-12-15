namespace Library.Application.Contracts.Issues;

/// <summary>
/// Application service contract for working with book issues.
/// </summary>
public interface IIssueService : IApplicationService
{
    public Task<IssueDto?> GetAsync(int id);

    public Task<IReadOnlyList<IssueDto>> GetListAsync();

    public Task<IssueDto> CreateAsync(IssueCreateUpdateDto input);

    public Task<IssueDto> UpdateAsync(int id, IssueCreateUpdateDto input);

    public Task DeleteAsync(int id);
}
