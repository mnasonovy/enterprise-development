using System.Collections.Generic;
using System.Threading.Tasks;

using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;

using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с издателями через MongoDB EF Core
/// Заменяет старый MongoDBDriver подход на современный EF Core
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
    /// Получить издателя по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>Модель издателя или null если не найден</returns>
    public async Task<Publisher?> ReadAsync(int id)
    {
        return await _publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Получить список всех издателей из базы данных
    /// </summary>
    /// <returns>Список всех издателей</returns>
    public async Task<IReadOnlyList<Publisher>> ReadAllAsync()
    {
        var result = await _publishers
            .AsNoTracking()
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового издателя в базе данных
    /// </summary>
    /// <param name="entity">Модель издателя для сохранения</param>
    /// <returns>Созданный издатель с заполненными данными</returns>
    public async Task<Publisher> CreateAsync(Publisher entity)
    {
        await _publishers.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Удалить издателя из базы данных по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>true если издатель был удален, false если не найден</returns>
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
    /// Обновить данные существующего издателя
    /// </summary>
    /// <param name="entity">Модель издателя с обновленными данными</param>
    /// <returns>Обновленный издатель или null если не найден</returns>
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