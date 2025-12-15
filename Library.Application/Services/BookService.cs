using Library.Application.Contracts.Books;

namespace Library.Application.Services;

/// <summary>
/// Application service implementation for working with books.
/// </summary>
public class BookService : IBookService
{
    public Task<BookDto?> GetAsync(int id)
    {
        // TODO: implement using repository and mapping
        return Task.FromResult<BookDto?>(null);
    }

    public Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        // TODO: implement using repository and mapping
        IReadOnlyList<BookDto> result = Array.Empty<BookDto>();
        return Task.FromResult(result);
    }

    public Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        // TODO: implement using repository and mapping
        var result = new BookDto
        {
            Id = 0,
            Title = input.Title,
            Year = input.Year,
            AlphabetCode = input.AlphabetCode,
            BookTypeName = string.Empty,
            PublisherName = string.Empty,
            AuthorNames = []
        };

        return Task.FromResult(result);
    }

    public Task<BookDto> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        // TODO: implement using repository and mapping
        var result = new BookDto
        {
            Id = id,
            Title = input.Title,
            Year = input.Year,
            AlphabetCode = input.AlphabetCode,
            BookTypeName = string.Empty,
            PublisherName = string.Empty,
            AuthorNames = []
        };

        return Task.FromResult(result);
    }

    public Task DeleteAsync(int id)
    {
        // TODO: implement using repository
        return Task.CompletedTask;
    }
}
