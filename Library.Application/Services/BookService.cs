using AutoMapper;
using Library.Application.Contracts.Books;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления книгами в библиотечной системе.
/// Реализует CRUD операции и Upsert для синхронизации с NATS JetStream.
/// Автоматически подгружает связанные данные (авторы, тип, издатель).
/// </summary>
public class BookService(IBookRepository bookRepository, IMapper mapper) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получает книгу по идентификатору со всеми связанными данными.
    /// </summary>
    public async Task<BookDto?> GetAsync(int id)
    {
        var book = await _bookRepository.ReadAsync(id);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    /// <summary>
    /// Получает список всех книг.
    /// </summary>
    public async Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        var books = await _bookRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<BookDto>>(books);
    }

    /// <summary>
    /// Создаёт новую книгу.
    /// Требует существующих BookTypeId, PublisherId и AuthorIds.
    /// </summary>
    public async Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        var book = _mapper.Map<Book>(input);
        var created = await _bookRepository.CreateAsync(book);
        return _mapper.Map<BookDto>(created);
    }

    /// <summary>
    /// Обновляет существующую книгу.
    /// </summary>
    public async Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        var existing = await _bookRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _bookRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<BookDto>(updated);
    }

    /// <summary>
    /// Удаляет книгу и все её связи по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _bookRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Создаёт новую книгу или обновляет существующую (Upsert).
    /// Идемпотентная операция для синхронизации из NATS.
    /// </summary>
    public async Task<BookDto> UpsertAsync(BookCreateUpdateDto input)
    {
        var existing = await _bookRepository.ReadAsync(input.Id);

        if (existing != null)
        {
            _mapper.Map(input, existing);
            var updated = await _bookRepository.UpdateAsync(existing);
            return _mapper.Map<BookDto>(updated)!;
        }
        else
        {
            var book = _mapper.Map<Book>(input);
            var created = await _bookRepository.CreateAsync(book);
            return _mapper.Map<BookDto>(created);
        }
    }
}
