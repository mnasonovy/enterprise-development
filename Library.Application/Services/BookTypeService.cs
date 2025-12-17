using AutoMapper;
using Library.Application.Contracts.BookTypes;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над типами книг.
/// 🔧 ИСПРАВЛЕНО: Используется AutoMapper!
/// </summary>
public class BookTypeService : IBookTypeService
{
    private readonly BookTypeRepository _bookTypeRepository;
    private readonly IMapper _mapper;

    public BookTypeService(BookTypeRepository bookTypeRepository, IMapper mapper)
    {
        _bookTypeRepository = bookTypeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить тип книги по идентификатору.
    /// 🔧 Include загружает Books!
    /// </summary>
    public async Task<BookTypeDto?> GetAsync(int id)
    {
        var bookType = await _bookTypeRepository.ReadAsync(id);
        return bookType == null ? null : _mapper.Map<BookTypeDto>(bookType);
    }

    /// <summary>
    /// Получить список всех типов книг.
    /// 🔧 Include загружает Books!
    /// </summary>
    public async Task<IReadOnlyList<BookTypeDto>> GetListAsync()
    {
        var bookTypes = await _bookTypeRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<BookTypeDto>>(bookTypes);
    }

    /// <summary>
    /// Создать новый тип книги.
    /// </summary>
    public async Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input)
    {
        var bookType = _mapper.Map<BookType>(input);
        var created = await _bookTypeRepository.CreateAsync(bookType);
        return _mapper.Map<BookTypeDto>(created);
    }

    /// <summary>
    /// Обновить существующий тип книги.
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
    /// Удалить тип книги по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _bookTypeRepository.DeleteAsync(id);
    }
}
