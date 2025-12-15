using Library.Domain.Models;

namespace Library.Application.Contracts.Books;

public interface IBookService : IApplicationService
{
    public Task<BookDto?> GetAsync(int id);
    public Task<IReadOnlyList<BookDto>> GetListAsync();
    public Task<BookDto> CreateAsync(BookCreateUpdateDto input);
    public Task<BookDto> UpdateAsync(int id, BookCreateUpdateDto input);
    public Task DeleteAsync(int id);
}
