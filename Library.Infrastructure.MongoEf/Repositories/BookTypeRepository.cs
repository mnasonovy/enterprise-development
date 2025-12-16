using System.Collections.Generic;
using System.Threading.Tasks;

using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;

using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с типами книг через MongoDB EF Core
/// Заменяет старый MongoDBDriver подход на современный EF Core
/// </summary>
public class BookTypeRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<BookType> _bookTypes;

    public BookTypeRepository(MongoDbContext context)
    {
        _context = context;
        _bookTypes = context.BookTypes;
    }

    /// <summary>
    /// Получить тип книги по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>Модель типа книги или null если не найдена</returns>
    public async Task<BookType?> ReadAsync(int id)
    {
        return await _bookTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(bt => bt.Id == id);
    }

    /// <summary>
    /// Получить список всех типов книг из базы данных
    /// </summary>
    /// <returns>Список всех типов книг</returns>
    public async Task<IReadOnlyList<BookType>> ReadAllAsync()
    {
        var result = await _bookTypes
            .AsNoTracking()
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать новый тип книги в базе данных
    /// </summary>
    /// <param name="entity">Модель типа книги для сохранения</param>
    /// <returns>Созданный тип книги с заполненными данными</returns>
    public async Task<BookType> CreateAsync(BookType entity)
    {
        await _bookTypes.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Удалить тип книги из базы данных по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>true если тип книги был удален, false если не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _bookTypes.FirstOrDefaultAsync(bt => bt.Id == id);

        if (entity is null)
            return false;

        _bookTypes.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Обновить данные существующего типа книги
    /// </summary>
    /// <param name="entity">Модель типа книги с обновленными данными</param>
    /// <returns>Обновленный тип книги или null если не найден</returns>
    public async Task<BookType?> UpdateAsync(BookType entity)
    {
        var exists = await _bookTypes.AnyAsync(bt => bt.Id == entity.Id);

        if (!exists)
            return null;

        _bookTypes.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}