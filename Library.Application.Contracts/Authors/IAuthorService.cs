namespace Library.Application.Contracts.Authors;

/// <summary>
/// Application service contract for working with authors.
/// </summary>
public interface IAuthorService : IApplicationService
{
    public Task<AuthorDto?> GetAsync(int id);

    public Task<IReadOnlyList<AuthorDto>> GetListAsync();

    public Task<AuthorDto> CreateAsync(AuthorCreateUpdateDto input);

    public Task<AuthorDto> UpdateAsync(int id, AuthorCreateUpdateDto input);

    public Task DeleteAsync(int id);
}
