using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с типами книг через MongoDB EF Core.
/// Реализует паттерн Repository для инкапсуляции логики доступа к данным.
/// Поддерживает асинхронные операции CRUD через Entity Framework Core.
/// </summary>
public class BookTypeRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<BookType> _bookTypes;

    /// <summary>
    /// Инициализирует репозиторий с контекстом MongoDB EF Core.
    /// </summary>
    /// <param name="context">Контекст базы данных MongoDB</param>
    public BookTypeRepository(MongoDbContext context)
    {
        _context = context;
        _bookTypes = context.BookTypes;
    }

    /// <summary>
    /// Получить тип книги по идентификатору.
    /// MongoDB EF Core не поддерживает Include, поэтому загружаем только сам тип.
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
    /// MongoDB EF Core не поддерживает Include, поэтому загружаем только типы без навигаций.
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
        await _context.SaveChangesAsync();
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
        await _context.SaveChangesAsync();
        return true;
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
        await _context.SaveChangesAsync();
        return entity;
    }
}
