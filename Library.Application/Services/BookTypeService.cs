namespace Library.Application.Services;

using Library.Application.Contracts.BookTypes;

using Library.Domain.Models;

using Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Сервис для CRUD-операций над типами книг
/// </summary>
public class BookTypeService : IBookTypeService
{
    private readonly BookTypeRepository _bookTypeRepository;

    public BookTypeService(BookTypeRepository bookTypeRepository)
    {
        _bookTypeRepository = bookTypeRepository;
    }

    /// <summary>
    /// Получить тип книги по идентификатору
    /// </summary>
    public async Task<BookTypeDto?> GetAsync(int id)
    {
        var bookType = await _bookTypeRepository.ReadAsync(id);
        return bookType is null
            ? null
            : MapToDto(bookType);
    }

    /// <summary>
    /// Получить список всех типов книг
    /// </summary>
    public async Task<IReadOnlyList<BookTypeDto>> GetListAsync()
    {
        var bookTypes = await _bookTypeRepository.ReadAllAsync();
        return bookTypes.Select(MapToDto).ToList().AsReadOnly();
    }

    /// <summary>
    /// Создать новый тип книги
    /// </summary>
    public async Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input)
    {
        var bookType = new BookType
        {
            Name = input.Name
        };

        var created = await _bookTypeRepository.CreateAsync(bookType);
        return MapToDto(created);
    }

    /// <summary>
    /// Обновить существующий тип книги
    /// </summary>
    public async Task<BookTypeDto> UpdateAsync(int id, BookTypeCreateUpdateDto input)
    {
        var existing = await _bookTypeRepository.ReadAsync(id)
            ?? throw new InvalidOperationException($"BookType with id {id} was not found.");

        existing.Name = input.Name;

        var updated = await _bookTypeRepository.UpdateAsync(existing)
            ?? throw new InvalidOperationException($"BookType with id {id} was not updated.");

        return MapToDto(updated);
    }

    /// <summary>
    /// Удалить тип книги по идентификатору
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var deleted = await _bookTypeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"BookType with id {id} was not deleted.");
        }
    }

    /// <summary>
    /// Преобразовать Domain модель типа книги в DTO
    /// </summary>
    private static BookTypeDto MapToDto(BookType bookType) => new()
    {
        Id = bookType.Id,
        Name = bookType.Name
    };
}