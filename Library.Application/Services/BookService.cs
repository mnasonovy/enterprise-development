using Library.Application.Contracts.Books;
using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Repositories;

namespace Library.Application.Services;

public class BookService : IBookService
{
    private readonly BookMongoRepository _bookRepository;

    public BookService(BookMongoRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto?> GetAsync(int id)
    {
        var book = await _bookRepository.ReadAsync(id);

        return book is null
            ? null
            : MapToDto(book);
    }

    public async Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        var books = await _bookRepository.ReadAllAsync();
        return books.Select(MapToDto).ToArray();
    }
    public async Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        var book = new Book
        {
            Title = input.Title,
            Year = input.Year,
            AlphabetCode = input.AlphabetCode,
            BookType = new BookType { Id = input.BookTypeId, Name = string.Empty },
            Publisher = new Publisher { Id = input.PublisherId, Name = string.Empty },
            Authors = new List<Author>()
        };

        var created = await _bookRepository.CreateAsync(book);
        return MapToDto(created);
    }


    public async Task<BookDto> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        var existing = await _bookRepository.ReadAsync(id)
                       ?? throw new InvalidOperationException($"Book with id {id} was not found.");

        existing.Title = input.Title;
        existing.Year = input.Year;
        existing.AlphabetCode = input.AlphabetCode;
        existing.BookType = new BookType { Id = input.BookTypeId, Name = string.Empty };
        existing.Publisher = new Publisher { Id = input.PublisherId, Name = string.Empty };

        var updated = await _bookRepository.UpdateAsync(existing)
                      ?? throw new InvalidOperationException($"Book with id {id} was not updated.");

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await _bookRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Book with id {id} was not deleted.");
        }
    }

    private static BookDto MapToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Year = book.Year,
            AlphabetCode = book.AlphabetCode,
            BookTypeName = book.BookType.Name,
            PublisherName = book.Publisher.Name,
            AuthorNames = book.Authors.Select(a => a.LastName).ToList()
        };
    }
}
