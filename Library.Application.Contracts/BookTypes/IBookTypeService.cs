namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// Application service contract for working with book types.
/// </summary>
public interface IBookTypeService : IApplicationService
{
    Task<BookTypeDto?> GetAsync(int id);

    Task<IReadOnlyList<BookTypeDto>> GetListAsync();

    Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input);

    Task<BookTypeDto> UpdateAsync(int id, BookTypeCreateUpdateDto input);

    Task DeleteAsync(int id);
}
