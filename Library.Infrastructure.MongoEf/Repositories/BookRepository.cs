using System.Collections.Generic;
using System.Threading.Tasks;

using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;

using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с книгами через MongoDB EF Core
/// Заменяет старый MongoDBDriver подход на современный EF Core
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
    /// Получить книгу по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>Модель книги или null если не найдена</returns>
    public async Task<Book?> ReadAsync(int id)
    {
        return await _books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <summary>
    /// Получить список всех книг из базы данных
    /// </summary>
    /// <returns>Список всех книг</returns>
    public async Task<IReadOnlyList<Book>> ReadAllAsync()
    {
        var result = await _books
            .AsNoTracking()
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать новую книгу в базе данных
    /// </summary>
    /// <param name="entity">Модель книги для сохранения</param>
    /// <returns>Созданная книга с заполненными данными</returns>
    /// <remarks>
    /// Id задаётся вручную. Позже можно реализовать автоинкремент через индексы MongoDB
    /// </remarks>
    public async Task<Book> CreateAsync(Book entity)
    {
        await _books.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Удалить книгу из базы данных по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>true если книга была удалена, false если не найдена</returns>
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
    /// Обновить данные существующей книги
    /// </summary>
    /// <param name="entity">Модель книги с обновленными данными</param>
    /// <returns>Обновленная книга или null если не найдена</returns>
    public async Task<Book?> UpdateAsync(Book entity)
    {
        var exists = await _books.AnyAsync(b => b.Id == entity.Id);

        if (!exists)
            return null;

        _books.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}
