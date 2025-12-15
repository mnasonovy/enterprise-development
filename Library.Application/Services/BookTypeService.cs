using Library.Application.Contracts.BookTypes;
using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Repositories;

namespace Library.Application.Services;

public class BookTypeService : IBookTypeService
{
    private readonly BookTypeMongoRepository _bookTypeRepository;

    public BookTypeService(BookTypeMongoRepository bookTypeRepository)
    {
        _bookTypeRepository = bookTypeRepository;
    }

    public async Task<BookTypeDto?> GetAsync(int id)
    {
        var bookType = await _bookTypeRepository.ReadAsync(id);
        return bookType is null
            ? null
            : MapToDto(bookType);
    }

    public async Task<IReadOnlyList<BookTypeDto>> GetListAsync()
    {
        var bookTypes = await _bookTypeRepository.ReadAllAsync();
        return bookTypes.Select(MapToDto).ToArray();
    }

    public async Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input)
    {
        var bookType = new BookType
        {
            Name = input.Name
        };

        var created = await _bookTypeRepository.CreateAsync(bookType);
        return MapToDto(created);
    }

    public async Task<BookTypeDto> UpdateAsync(int id, BookTypeCreateUpdateDto input)
    {
        var existing = await _bookTypeRepository.ReadAsync(id)
            ?? throw new InvalidOperationException($"BookType with id {id} was not found.");

        existing.Name = input.Name;

        var updated = await _bookTypeRepository.UpdateAsync(existing)
            ?? throw new InvalidOperationException($"BookType with id {id} was not updated.");

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await _bookTypeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"BookType with id {id} was not deleted.");
        }
    }

    private static BookTypeDto MapToDto(BookType bookType) => new()
    {
        Id = bookType.Id,
        Name = bookType.Name
    };
}