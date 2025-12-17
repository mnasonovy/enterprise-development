using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с книгами через MongoDB EF Core.
/// Заменяет старый MongoDBDriver подход на современный EF Core.
/// 🔧 ИСПРАВЛЕНО: Используются реальные свойства Book!
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
    /// Получить книгу по идентификатору.
    /// 🔧 ИСПРАВЛЕНО: Include для Authors, BookType, Publisher
    /// </summary>
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
    /// Получить список всех книг из базы данных.
    /// 🔧 ИСПРАВЛЕНО: Include для всех связанных сущностей
    /// </summary>
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
    /// </summary>
    public async Task<Book> CreateAsync(Book entity)
    {
        await _books.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить книгу из базы данных по идентификатору.
    /// </summary>
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
    /// Обновить данные существующей книги.
    /// 🔧 ИСПРАВЛЕНО: Используются РЕАЛЬНЫЕ свойства Book!
    /// </summary>
    public async Task<Book?> UpdateAsync(Book entity)
    {
        var existing = await _books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == entity.Id);

        if (existing is null)
            return null;

        // 🔧 Обновляем только существующие свойства
        existing.Title = entity.Title;
        existing.Year = entity.Year;
        existing.AlphabetCode = entity.AlphabetCode;
        existing.BookTypeId = entity.BookTypeId;
        existing.PublisherId = entity.PublisherId;

        // 🔧 Обновляем Authors если они изменились
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
