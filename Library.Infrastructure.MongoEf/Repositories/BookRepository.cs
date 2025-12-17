using System.Collections.Generic;
using System.Threading.Tasks;

using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;

using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с книгами через MongoDB EF Core.
/// Реализует паттерн Repository для абстрагирования логики доступа к данным.
/// Предоставляет методы CRUD для управления сущностями Book в базе данных.
/// </summary>
public class BookRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<Book> _books;

    public BookRepository(MongoDbContext context)
    {
        _context = context;
        _books = context.Books;
    }

    /// <summary>
    /// Получить информацию о книге по её идентификатору.
    /// Включает связанные сущности: Authors, BookType и Publisher.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги в базе данных.</param>
    /// <returns>Сущность Book с полностью загруженными связанными данными или null.</returns>
    public async Task<Book?> ReadAsync(int id)
    {
        return await _books
            .AsNoTracking()
            .Include(b => b.Authors)
            .Include(b => b.BookType)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <summary>
    /// Получить полный список всех книг из базы данных.
    /// Автоматически загружает все связанные сущности для каждой книги.
    /// </summary>
    /// <returns>Неизменяемая коллекция всех книг с загруженными связанными данными.</returns>
    public async Task<IReadOnlyList<Book>> ReadAllAsync()
    {
        var result = await _books
            .AsNoTracking()
            .Include(b => b.Authors)
            .Include(b => b.BookType)
            .Include(b => b.Publisher)
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать новую книгу в базе данных.
    /// Сохраняет сущность Book со всеми её свойствами и связями.
    /// </summary>
    /// <param name="entity">Сущность Book с заполненными данными для создания.</param>
    /// <returns>Созданная сущность Book с заполненным ID.</returns>
    public async Task<Book> CreateAsync(Book entity)
    {
        await _books.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Удалить книгу из базы данных по её идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления.</param>
    /// <returns>true, если книга была успешно удалена; false, если книга не найдена.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _books.FirstOrDefaultAsync(b => b.Id == id);

        if (entity is null)
            return false;

        _books.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Обновить данные существующей книги в базе данных.
    /// Обновляет все основные свойства и пересчитывает связь "многие-ко-многим" с авторами.
    /// </summary>
    /// <param name="entity">Сущность Book с обновленными данными.</param>
    /// <returns>Обновленная сущность Book или null, если книга с таким ID не найдена.</returns>
    public async Task<Book?> UpdateAsync(Book entity)
    {
        var existing = await _books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == entity.Id);

        if (existing is null)
            return null;

        // Обновляем основные свойства книги
        existing.Title = entity.Title;
        existing.Year = entity.Year;
        existing.AlphabetCode = entity.AlphabetCode;
        existing.BookTypeId = entity.BookTypeId;
        existing.PublisherId = entity.PublisherId;

        // Обновляем коллекцию авторов если она изменилась
        if (entity.Authors != existing.Authors)
        {
            existing.Authors.Clear();
            foreach (var author in entity.Authors)
            {
                existing.Authors.Add(author);
            }
        }

        _books.Update(existing);
        await _context.SaveChangesAsync();

        return existing;
    }
}
