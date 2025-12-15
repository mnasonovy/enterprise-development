namespace Library.Application.Contracts.Authors;

/// <summary>
/// Application service contract for working with authors.
/// </summary>
public interface IAuthorService : IApplicationService
{
    Task<AuthorDto?> GetAsync(int id);

    Task<IReadOnlyList<AuthorDto>> GetListAsync();

    Task<AuthorDto> CreateAsync(AuthorCreateUpdateDto input);

    Task<AuthorDto> UpdateAsync(int id, AuthorCreateUpdateDto input);

    Task DeleteAsync(int id);
}
