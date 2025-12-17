using AutoMapper;

using Library.Application.Contracts.Books;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис приложения для управления книгами.
/// Реализует логику CRUD-операций с использованием AutoMapper для трансформации данных.
/// Работает через BookRepository для доступа к данным из MongoDB.
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
    /// Получить информацию о книге по её идентификатору.
    /// Включает связанные данные: авторов, тип книги и издателя.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги.</param>
    /// <returns>DTO книги или null, если книга не найдена.</returns>
    public async Task<BookDto?> GetAsync(int id)
    {
        var book = await _bookRepository.ReadAsync(id);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    /// <summary>
    /// Получить полный список всех книг из базы данных.
    /// Каждая книга содержит полные данные о связанных сущностях.
    /// </summary>
    /// <returns>Коллекция DTO всех книг.</returns>
    public async Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        var books = await _bookRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<BookDto>>(books);
    }

    /// <summary>
    /// Создать новую книгу в каталоге библиотеки.
    /// Устанавливает связи через внешние ключи (BookTypeId, PublisherId).
    /// </summary>
    /// <param name="input">DTO с данными новой книги.</param>
    /// <returns>DTO созданной книги с автоматически заполненным ID.</returns>
    public async Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        var book = _mapper.Map<Book>(input);
        // Устанавливаем внешние ключи для связи с типом и издателем
        book.BookTypeId = input.BookTypeId;
        book.PublisherId = input.PublisherId;

        var created = await _bookRepository.CreateAsync(book);
        return _mapper.Map<BookDto>(created);
    }

    /// <summary>
    /// Обновить информацию о существующей книге.
    /// Обновляет все поля и пересчитывает связи через внешние ключи.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для обновления.</param>
    /// <param name="input">DTO с новыми данными книги.</param>
    /// <returns>DTO обновленной книги или null, если книга не найдена.</returns>
    public async Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        var existing = await _bookRepository.ReadAsync(id);
        if (existing == null)
            return null;

        // Обновляем основные поля и внешние ключи
        existing.Title = input.Title;
        existing.Year = input.Year;
        existing.AlphabetCode = input.AlphabetCode;
        existing.BookTypeId = input.BookTypeId;
        existing.PublisherId = input.PublisherId;

        var updated = await _bookRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<BookDto>(updated);
    }

    /// <summary>
    /// Удалить книгу из каталога библиотеки по её идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления.</param>
    /// <returns>Асинхронная задача удаления.</returns>
    public async Task DeleteAsync(int id)
    {
        await _bookRepository.DeleteAsync(id);
    }
}
