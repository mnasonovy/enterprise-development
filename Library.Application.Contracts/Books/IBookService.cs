using Library.Domain.Models;

namespace Library.Application.Contracts.Books;

/// <summary>
/// Application service contract for working with books.
/// </summary>
public interface IBookService : IApplicationService
{
    Task<BookDto?> GetAsync(int id);

    Task<IReadOnlyList<BookDto>> GetListAsync();

    Task<BookDto> CreateAsync(BookCreateUpdateDto input);

    Task<BookDto> UpdateAsync(int id, BookCreateUpdateDto input);

    Task DeleteAsync(int id);
}
