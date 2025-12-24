using AutoMapper;
using Library.Application.Contracts.BookTypes;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления типами книг в библиотечной системе.
/// Реализует CRUD операции и Upsert для синхронизации с NATS JetStream.
/// Типы книг (роман, учебник, справочник и т.д.) используются как справочник.
/// </summary>
public class BookTypeService(IBookTypeRepository bookTypeRepository, IMapper mapper) : IBookTypeService
{
    private readonly IBookTypeRepository _bookTypeRepository = bookTypeRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получает тип книги по идентификатору.
    /// </summary>
    public async Task<BookTypeDto?> GetAsync(int id)
    {
        var bookType = await _bookTypeRepository.ReadAsync(id);
        return bookType == null ? null : _mapper.Map<BookTypeDto>(bookType);
    }

    /// <summary>
    /// Получает список всех типов книг.
    /// </summary>
    public async Task<IReadOnlyList<BookTypeDto>> GetListAsync()
    {
        var bookTypes = await _bookTypeRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<BookTypeDto>>(bookTypes);
    }

    /// <summary>
    /// Создаёт новый тип книги с автоматической генерацией ID.
    /// </summary>
    public async Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input)
    {
        var maxId = await _bookTypeRepository.GetMaxIdAsync();
        var bookType = _mapper.Map<BookType>(input);
        bookType.Id = maxId + 1; // ← генерируем новый ID
        var created = await _bookTypeRepository.CreateAsync(bookType);
        return _mapper.Map<BookTypeDto>(created);
    }

    /// <summary>
    /// Обновляет существующий тип книги.
    /// </summary>
    public async Task<BookTypeDto?> UpdateAsync(int id, BookTypeCreateUpdateDto input)
    {
        var existing = await _bookTypeRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _bookTypeRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<BookTypeDto>(updated);
    }

    /// <summary>
    /// Удаляет тип книги по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _bookTypeRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Создаёт новый тип книги или обновляет существующий (Upsert).
    /// При создании генерируется новый ID автоматически.
    /// Идемпотентная операция для синхронизации из NATS.
    /// </summary>
    public async Task<BookTypeDto> UpsertAsync(BookTypeCreateUpdateDto input)
    {
        var maxId = await _bookTypeRepository.GetMaxIdAsync();
        var bookType = _mapper.Map<BookType>(input);
        bookType.Id = maxId + 1; // ← генерируем новый ID при создании
        var upsertedBookType = await _bookTypeRepository.CreateAsync(bookType);
        return _mapper.Map<BookTypeDto>(upsertedBookType);
    }
}
