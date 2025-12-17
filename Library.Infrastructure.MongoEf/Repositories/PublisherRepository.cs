using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с издателями через MongoDB EF Core.
/// Заменяет старый MongoDBDriver подход на современный EF Core.
/// НЕ использует Include() для совместимости с MongoDB (нет foreign keys).
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
    /// Использует AsNoTracking для оптимизации при только чтении данных.
    /// БЕЗ Include() - MongoDB не поддерживает foreign keys и eager loading.
    /// </summary>
    /// <param name="id">Идентификатор издателя</param>
    /// <returns>Publisher если найден, иначе null</returns>
    public async Task<Publisher?> ReadAsync(int id)
    {
        return await _publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Получить список всех издателей из базы данных.
    /// AsNoTracking улучшает производительность при чтении большого списка.
    /// БЕЗ Include() - MongoDB не поддерживает foreign keys и eager loading.
    /// Возвращает читаемый (доступный только для чтения) список.
    /// </summary>
    /// <returns>Неизменяемый список всех издателей</returns>
    public async Task<IReadOnlyList<Publisher>> ReadAllAsync()
    {
        var result = await _publishers
            .AsNoTracking()
            .ToListAsync();
        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового издателя в базе данных.
    /// Добавляет издателя в DbSet, затем сохраняет изменения в MongoDB.
    /// Возвращает созданного издателя с установленным Id.
    /// </summary>
    /// <param name="entity">Сущность издателя для создания</param>
    /// <returns>Созданная сущность Publisher</returns>
    public async Task<Publisher> CreateAsync(Publisher entity)
    {
        await _publishers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить издателя из базы данных по идентификатору.
    /// Проверяет существование издателя перед удалением.
    /// Возвращает true если удаление успешно, false если издатель не найден.
    /// </summary>
    /// <param name="id">Идентификатор издателя для удаления</param>
    /// <returns>True если удалено, false если не найдено</returns>
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
    /// Проверяет наличие издателя перед обновлением.
    /// Возвращает обновленного издателя если успешно, null если издатель не найден.
    /// </summary>
    /// <param name="entity">Сущность с обновленными данными</param>
    /// <returns>Обновленный Publisher или null если издатель не найден</returns>
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
