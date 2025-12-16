using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;

using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с авторами через MongoDB EF Core
/// Заменяет старый MongoDBDriver подход на современный EF Core
/// </summary>
public class AuthorRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<Author> _authors;

    public AuthorRepository(MongoDbContext context)
    {
        _context = context;
        _authors = context.Authors;
    }

    /// <summary>
    /// Получить автора по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора</param>
    /// <returns>Модель автора или null если не найден</returns>
    public async Task<Author?> ReadAsync(int id)
    {
        return await _authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Получить список всех авторов из базы данных
    /// </summary>
    /// <returns>Неизменяемый список всех авторов</returns>
    public async Task<IReadOnlyList<Author>> ReadAllAsync()
    {
        var result = await _authors
            .AsNoTracking()
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового автора в базе данных
    /// </summary>
    /// <param name="entity">Модель автора для сохранения</param>
    /// <returns>Созданный автор с заполненными данными</returns>
    public async Task<Author> CreateAsync(Author entity)
    {
        await _authors.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Удалить автора из базы данных по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора</param>
    /// <returns>true если автор был удален, false если не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _authors.FirstOrDefaultAsync(a => a.Id == id);

        if (entity is null)
            return false;

        _authors.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Обновить данные существующего автора
    /// </summary>
    /// <param name="entity">Модель автора с обновленными данными</param>
    /// <returns>Обновленный автор или null если не найден</returns>
    public async Task<Author?> UpdateAsync(Author entity)
    {
        var exists = await _authors.AnyAsync(a => a.Id == entity.Id);

        if (!exists)
            return null;

        _authors.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}