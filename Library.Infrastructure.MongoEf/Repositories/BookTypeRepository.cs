using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с типами книг через MongoDB EF Core.
/// Реализует интерфейс IBookTypeRepository для инкапсуляции логики доступа к данным.
/// Поддерживает асинхронные операции CRUD через Entity Framework Core.
/// </summary>
public class BookTypeRepository(MongoDbContext context) : IBookTypeRepository
{
    private readonly DbSet<BookType> _bookTypes = context.BookTypes;

    /// <summary>
    /// Получить тип книги по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>Объект BookType или null если тип не найден</returns>
    public async Task<BookType?> ReadAsync(int id)
    {
        return await _bookTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(bt => bt.Id == id);
    }

    /// <summary>
    /// Получить список всех типов книг из базы данных.
    /// </summary>
    /// <returns>Неизменяемый список всех типов книг</returns>
    public async Task<IReadOnlyList<BookType>> ReadAllAsync()
    {
        var result = await _bookTypes
            .AsNoTracking()
            .ToListAsync();
        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать новый тип книги в базе данных.
    /// </summary>
    /// <param name="entity">Объект BookType для сохранения</param>
    /// <returns>Созданный объект BookType с заполненным Id</returns>
    public async Task<BookType> CreateAsync(BookType entity)
    {
        await _bookTypes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Обновить данные существующего типа книги.
    /// </summary>
    /// <param name="entity">Объект BookType с обновленными данными</param>
    /// <returns>Обновленный объект BookType или null если тип не найден</returns>
    public async Task<BookType?> UpdateAsync(BookType entity)
    {
        var exists = await _bookTypes.AnyAsync(bt => bt.Id == entity.Id);
        if (!exists)
            return null;

        _bookTypes.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить тип книги из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги для удаления</param>
    /// <returns>true если удаление успешно, false если тип не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _bookTypes.FirstOrDefaultAsync(bt => bt.Id == id);
        if (entity is null)
            return false;

        _bookTypes.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
