using AutoMapper;
using Library.Application.Contracts.Books;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над книгами.
/// 🔧 ИСПРАВЛЕНО: AutoMapper + правильная работа с FK!
/// </summary>
public class BookService : IBookService
{
    private readonly BookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BookService(BookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить книгу по идентификатору.
    /// 🔧 Include загружает Authors, BookType, Publisher!
    /// </summary>
    public async Task<BookDto?> GetAsync(int id)
    {
        var book = await _bookRepository.ReadAsync(id);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    /// <summary>
    /// Получить список всех книг.
    /// 🔧 Include загружает Authors, BookType, Publisher!
    /// </summary>
    public async Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        var books = await _bookRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<BookDto>>(books);
    }

    /// <summary>
    /// Создать новую книгу.
    /// 🔧 ИСПРАВЛЕНО: Используются FK вместо создания новых объектов!
    /// </summary>
    public async Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        var book = _mapper.Map<Book>(input);
        // 🔧 ВАЖНО: Устанавливаем FK, а не создаём новые объекты!
        book.BookTypeId = input.BookTypeId;
        book.PublisherId = input.PublisherId;

        var created = await _bookRepository.CreateAsync(book);
        return _mapper.Map<BookDto>(created);
    }

    /// <summary>
    /// Обновить существующую книгу.
    /// 🔧 ИСПРАВЛЕНО: Используются FK вместо создания новых объектов!
    /// </summary>
    public async Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        var existing = await _bookRepository.ReadAsync(id);
        if (existing == null)
            return null;

        // 🔧 Обновляем только базовые поля и FK!
        existing.Title = input.Title;
        existing.Year = input.Year;
        existing.AlphabetCode = input.AlphabetCode;
        existing.BookTypeId = input.BookTypeId;
        existing.PublisherId = input.PublisherId;

        var updated = await _bookRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<BookDto>(updated);
    }

    /// <summary>
    /// Удалить книгу по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _bookRepository.DeleteAsync(id);
    }
}
