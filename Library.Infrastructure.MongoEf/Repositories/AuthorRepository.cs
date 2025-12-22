using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для управления авторами в MongoDB через Entity Framework Core.
/// Предоставляет методы для выполнения CRUD операций над сущностью Author.
/// MongoDB EF Core не поддерживает Include, поэтому загружаем данные без навигаций.
/// </summary>
public class AuthorRepository(MongoDbContext context) : IAuthorRepository
{
    private readonly DbSet<Author> _authors = context.Authors;

    /// <summary>
    /// Получает автора по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для поиска.</param>
    /// <returns>Модель Author, если найдена; null если автор не существует.</returns>
    public async Task<Author?> ReadAsync(int id)
    {
        return await _authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Получает список всех авторов из базы данных.
    /// </summary>
    /// <returns>Коллекция всех авторов. Если авторов нет, возвращает пустой список.</returns>
    public async Task<IReadOnlyList<Author>> ReadAllAsync()
    {
        var result = await _authors
            .AsNoTracking()
            .ToListAsync();
        return result.AsReadOnly();
    }

    /// <summary>
    /// Создаёт нового автора в базе данных.
    /// </summary>
    /// <param name="entity">Модель Author с данными нового автора.</param>
    /// <returns>Созданный Author с назначенным идентификатором.</returns>
    public async Task<Author> CreateAsync(Author entity)
    {
        await _authors.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Обновляет информацию об существующем авторе.
    /// </summary>
    /// <param name="entity">Модель Author с обновленными данными.</param>
    /// <returns>Обновленный Author, если успешно; null если автор не найден.</returns>
    public async Task<Author?> UpdateAsync(Author entity)
    {
        var exists = await _authors.AnyAsync(a => a.Id == entity.Id);
        if (!exists)
            return null;

        _authors.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаляет автора из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    /// <returns>true если автор был успешно удален; false если автор не найден.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _authors.FirstOrDefaultAsync(a => a.Id == id);
        if (entity is null)
            return false;

        _authors.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}