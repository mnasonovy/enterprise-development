using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с издателями через MongoDB EF Core.
/// Реализует интерфейс IPublisherRepository для инкапсуляции логики доступа к данным.
/// Поддерживает асинхронные операции CRUD через Entity Framework Core.
/// </summary>
public class PublisherRepository(MongoDbContext context) : IPublisherRepository
{
    private readonly DbSet<Publisher> _publishers = context.Publishers;

    /// <summary>
    /// Получить издателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>Объект Publisher или null если издатель не найден</returns>
    public async Task<Publisher?> ReadAsync(int id)
    {
        return await _publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Получить список всех издателей из базы данных.
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
    /// </summary>
    /// <param name="entity">Объект Publisher для сохранения</param>
    /// <returns>Созданный объект Publisher с заполненным Id</returns>
    public async Task<Publisher> CreateAsync(Publisher entity)
    {
        await _publishers.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Обновить данные существующего издателя.
    /// </summary>
    /// <param name="entity">Объект Publisher с обновленными данными</param>
    /// <returns>Обновленный объект Publisher или null если издатель не найден</returns>
    public async Task<Publisher?> UpdateAsync(Publisher entity)
    {
        var exists = await _publishers.AnyAsync(p => p.Id == entity.Id);
        if (!exists)
            return null;

        _publishers.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить издателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для удаления</param>
    /// <returns>true если удаление успешно, false если издатель не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _publishers.FirstOrDefaultAsync(p => p.Id == id);
        if (entity is null)
            return false;

        _publishers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
