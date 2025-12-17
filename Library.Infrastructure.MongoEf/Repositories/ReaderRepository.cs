using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с читателями через MongoDB EF Core.
/// Предоставляет методы для выполнения CRUD операций над сущностью Reader.
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
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>Сущность Reader если найдена; null если читатель не существует</returns>
    public async Task<Reader?> ReadAsync(int id)
    {
        return await _readers
            .AsNoTracking()
            .Include(r => r.Issues)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Получить список всех читателей из базы данных.
    /// </summary>
    /// <returns>Неизменяемый список всех читателей с загруженными Issues</returns>
    public async Task<IReadOnlyList<Reader>> ReadAllAsync()
    {
        var result = await _readers
            .AsNoTracking()
            .Include(r => r.Issues)
            .ToListAsync();
        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового читателя в базе данных.
    /// </summary>
    /// <param name="entity">Сущность Reader для сохранения</param>
    /// <returns>Созданная сущность Reader с заполненным идентификатором</returns>
    public async Task<Reader> CreateAsync(Reader entity)
    {
        await _readers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить читателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    /// <returns>true если читатель успешно удален; false если читатель не найден</returns>
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
    /// <param name="entity">Сущность Reader с обновленными данными</param>
    /// <returns>Обновленная сущность Reader; null если читатель не найден</returns>
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
