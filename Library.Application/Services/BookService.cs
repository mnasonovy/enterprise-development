using AutoMapper;
using Library.Application.Contracts.Books;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над книгами.
/// Авторы, тип и издатель загружаются в репозитории.
/// </summary>
public class BookService(IBookRepository bookRepository, IMapper mapper) : IBookService
{
    public async Task<BookDto?> GetAsync(int id)
    {
        var book = await bookRepository.ReadAsync(id);
        return book == null ? null : mapper.Map<BookDto>(book);
    }

    public async Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        var books = await bookRepository.ReadAllAsync();
        return mapper.Map<IReadOnlyList<BookDto>>(books);
    }

    public async Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        var book = mapper.Map<Book>(input);
        var created = await bookRepository.CreateAsync(book);
        return mapper.Map<BookDto>(created);
    }

    public async Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        var existing = await bookRepository.ReadAsync(id);
        if (existing == null)
            return null;

        mapper.Map(input, existing);

        var updated = await bookRepository.UpdateAsync(existing);
        return updated == null ? null : mapper.Map<BookDto>(updated);
    }

    public async Task DeleteAsync(int id)
    {
        await bookRepository.DeleteAsync(id);
    }
}
