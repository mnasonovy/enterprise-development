using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с издателями через MongoDB EF Core.
/// Заменяет старый MongoDBDriver подход на современный EF Core.
/// 🔧 ИСПРАВЛЕНО: Добавлены .Include() для Books!
/// </summary>
public class PublisherRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<Publisher> _publishers;

    public PublisherRepository(MongoDbContext context)
    {
        _context = context;
        _publishers = context.Publishers;
    }

    /// <summary>
    /// Получить издателя по идентификатору.
    /// 🔧 ИСПРАВЛЕНО: Include для Books
    /// </summary>
    public async Task<Publisher?> ReadAsync(int id)
    {
        return await _publishers
            .AsNoTracking()
            .Include(p => p.Books)  // 🔧 ДОБАВЛЕНО
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Получить список всех издателей из базы данных.
    /// 🔧 ИСПРАВЛЕНО: Include для Books
    /// </summary>
    public async Task<IReadOnlyList<Publisher>> ReadAllAsync()
    {
        var result = await _publishers
            .AsNoTracking()
            .Include(p => p.Books)  // 🔧 ДОБАВЛЕНО
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового издателя в базе данных.
    /// </summary>
    public async Task<Publisher> CreateAsync(Publisher entity)
    {
        await _publishers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить издателя из базы данных по идентификатору.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _publishers.FirstOrDefaultAsync(p => p.Id == id);
        if (entity is null)
            return false;

        _publishers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Обновить данные существующего издателя.
    /// </summary>
    public async Task<Publisher?> UpdateAsync(Publisher entity)
    {
        var exists = await _publishers.AnyAsync(p => p.Id == entity.Id);
        if (!exists)
            return null;

        _publishers.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
