using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для управления книгами в каталоге библиотеки.
/// Обеспечивает полный набор CRUD операций и управление связями с авторами через коллекцию AuthorIds.
/// Авторы загружаются динамически при получении книги из БД.
/// </summary>
public class BookRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<Book> _books;
    private readonly DbSet<Author> _authors;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BookRepository"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных MongoDB Entity Framework Core</param>
    public BookRepository(MongoDbContext context)
    {
        _context = context;
        _books = context.Books;
        _authors = context.Authors;
    }

    /// <summary>
    /// Получает книгу по идентификатору с полной информацией об авторах.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>Объект книги с загруженными авторами или <c>null</c> если книга не найдена</returns>
    public async Task<Book?> ReadAsync(int id)
    {
        var book = await _books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return null;

        book.Authors = await LoadAuthorsAsync(book.AuthorIds ?? new List<int>());
        return book;
    }

    /// <summary>
    /// Получает все книги из каталога с полной информацией об авторах каждой книги.
    /// </summary>
    /// <returns>Доступная только для чтения коллекция всех книг с загруженными авторами</returns>
    public async Task<IReadOnlyCollection<Book>> ReadAllAsync()
    {
        var books = await _books
            .AsNoTracking()
            .ToListAsync();

        foreach (var book in books)
        {
            book.Authors = await LoadAuthorsAsync(book.AuthorIds ?? new List<int>());
        }

        return books.AsReadOnly();
    }

    /// <summary>
    /// Создает новую книгу в каталоге библиотеки.
    /// Сохраняет только идентификаторы авторов в поле <see cref="Book.AuthorIds"/>.
    /// Полные данные авторов загружаются динамически при чтении.
    /// </summary>
    /// <param name="entity">Объект книги для создания</param>
    /// <returns>Созданная книга с загруженными авторами из БД</returns>
    public async Task<Book> CreateAsync(Book entity)
    {
        _context.Books.Add(entity);
        await _context.SaveChangesAsync();

        entity.Authors = await LoadAuthorsAsync(entity.AuthorIds ?? new List<int>());
        return entity;
    }

    /// <summary>
    /// Обновляет существующую книгу в каталоге.
    /// Сохраняет только идентификаторы авторов в поле <see cref="Book.AuthorIds"/>.
    /// Полные данные авторов загружаются динамически при чтении.
    /// </summary>
    /// <param name="entity">Объект книги с обновленными данными</param>
    /// <returns>Обновленная книга с загруженными авторами или <c>null</c> если не удалось обновить</returns>
    public async Task<Book?> UpdateAsync(Book entity)
    {
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();

        entity.Authors = await LoadAuthorsAsync(entity.AuthorIds ?? new List<int>());
        return entity;
    }

    /// <summary>
    /// Удаляет книгу из каталога по идентификатору.
    /// При удалении также удаляются все связанные выпуски (экземпляры) книги.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления</param>
    public async Task DeleteAsync(int id)
    {
        var book = await _books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is not null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Загружает полную информацию об авторах по их идентификаторам из БД.
    /// Вспомогательный метод для динамической загрузки данных авторов.
    /// </summary>
    /// <param name="authorIds">Коллекция идентификаторов авторов для загрузки</param>
    /// <returns>Коллекция авторов с полной информацией (Initials, LastName и т.д.)</returns>
    private async Task<List<Author>> LoadAuthorsAsync(List<int> authorIds)
    {
        if (authorIds is null || authorIds.Count == 0)
            return new List<Author>();

        return await _authors
            .AsNoTracking()
            .Where(a => authorIds.Contains(a.Id))
            .ToListAsync();
    }
}
