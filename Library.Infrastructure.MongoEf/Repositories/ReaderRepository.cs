using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с читателями через MongoDB EF Core.
/// Реализует интерфейс IReaderRepository для инкапсуляции логики доступа к данным.
/// Поддерживает асинхронные операции CRUD через Entity Framework Core.
/// </summary>
public class ReaderRepository(MongoDbContext context) : IReaderRepository
{
    private readonly DbSet<Reader> _readers = context.Readers;

    /// <summary>
    /// Получить читателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>Объект Reader или null если читатель не найден</returns>
    public async Task<Reader?> ReadAsync(int id)
    {
        return await _readers
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Получить список всех читателей из базы данных.
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
    /// </summary>
    /// <param name="entity">Объект Reader для сохранения</param>
    /// <returns>Созданный объект Reader с заполненным Id</returns>
    public async Task<Reader> CreateAsync(Reader entity)
    {
        await _readers.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Обновить данные существующего читателя.
    /// </summary>
    /// <param name="entity">Объект Reader с обновленными данными</param>
    /// <returns>Обновленный объект Reader или null если читатель не найден</returns>
    public async Task<Reader?> UpdateAsync(Reader entity)
    {
        var exists = await _readers.AnyAsync(r => r.Id == entity.Id);
        if (!exists)
            return null;

        _readers.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить читателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    /// <returns>true если удаление успешно, false если читатель не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _readers.FirstOrDefaultAsync(r => r.Id == id);
        if (entity is null)
            return false;

        _readers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
