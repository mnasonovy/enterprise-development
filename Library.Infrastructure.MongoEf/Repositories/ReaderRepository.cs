using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с читателями через MongoDB EF Core.
/// Заменяет старый MongoDBDriver подход на современный EF Core.
/// 🔧 ИСПРАВЛЕНО: Добавлены .Include() для Issues!
/// </summary>
public class ReaderRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<Reader> _readers;

    public ReaderRepository(MongoDbContext context)
    {
        _context = context;
        _readers = context.Readers;
    }

    /// <summary>
    /// Получить читателя по идентификатору.
    /// 🔧 ИСПРАВЛЕНО: Include для Issues
    /// </summary>
    public async Task<Reader?> ReadAsync(int id)
    {
        return await _readers
            .AsNoTracking()
            .Include(r => r.Issues)  // 🔧 ДОБАВЛЕНО
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Получить список всех читателей из базы данных.
    /// 🔧 ИСПРАВЛЕНО: Include для Issues
    /// </summary>
    public async Task<IReadOnlyList<Reader>> ReadAllAsync()
    {
        var result = await _readers
            .AsNoTracking()
            .Include(r => r.Issues)  // 🔧 ДОБАВЛЕНО
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового читателя в базе данных.
    /// </summary>
    public async Task<Reader> CreateAsync(Reader entity)
    {
        await _readers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить читателя из базы данных по идентификатору.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _readers.FirstOrDefaultAsync(r => r.Id == id);
        if (entity is null)
            return false;

        _readers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Обновить данные существующего читателя.
    /// </summary>
    public async Task<Reader?> UpdateAsync(Reader entity)
    {
        var exists = await _readers.AnyAsync(r => r.Id == entity.Id);
        if (!exists)
            return null;

        _readers.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
