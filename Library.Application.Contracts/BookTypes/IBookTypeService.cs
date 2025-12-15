namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// Application service contract for working with book types.
/// </summary>
public interface IBookTypeService : IApplicationService
{
    public Task<BookTypeDto?> GetAsync(int id);

    public Task<IReadOnlyList<BookTypeDto>> GetListAsync();

    public Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input);

    public Task<BookTypeDto> UpdateAsync(int id, BookTypeCreateUpdateDto input);

    public Task DeleteAsync(int id);
}
