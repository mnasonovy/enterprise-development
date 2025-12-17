using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с читателями через MongoDB EF Core.
/// Заменяет старый MongoDBDriver подход на современный EF Core.
/// НЕ использует Include() для совместимости с MongoDB (нет foreign keys).
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
    /// Использует AsNoTracking для оптимизации при только чтении данных.
    /// БЕЗ Include() - MongoDB не поддерживает foreign keys и eager loading.
    /// </summary>
    /// <param name="id">Идентификатор читателя</param>
    /// <returns>Reader если найден, иначе null</returns>
    public async Task<Reader?> ReadAsync(int id)
    {
        return await _readers
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Получить список всех читателей из базы данных.
    /// AsNoTracking улучшает производительность при чтении большого списка.
    /// БЕЗ Include() - MongoDB не поддерживает foreign keys и eager loading.
    /// Возвращает читаемый (доступный только для чтения) список.
    /// </summary>
    /// <returns>Неизменяемый список всех читателей</returns>
    public async Task<IReadOnlyList<Reader>> ReadAllAsync()
    {
        var result = await _readers
            .AsNoTracking()
            .ToListAsync();
        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового читателя в базе данных.
    /// Добавляет читателя в DbSet, затем сохраняет изменения в MongoDB.
    /// Возвращает созданного читателя с установленным Id.
    /// </summary>
    /// <param name="entity">Сущность читателя для создания</param>
    /// <returns>Созданная сущность Reader</returns>
    public async Task<Reader> CreateAsync(Reader entity)
    {
        await _readers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить читателя из базы данных по идентификатору.
    /// Проверяет существование читателя перед удалением.
    /// Возвращает true если удаление успешно, false если читатель не найден.
    /// </summary>
    /// <param name="id">Идентификатор читателя для удаления</param>
    /// <returns>True если удалено, false если не найдено</returns>
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
    /// Проверяет наличие читателя перед обновлением.
    /// Возвращает обновленного читателя если успешно, null если читатель не найден.
    /// </summary>
    /// <param name="entity">Сущность с обновленными данными</param>
    /// <returns>Обновленный Reader или null если читатель не найден</returns>
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
