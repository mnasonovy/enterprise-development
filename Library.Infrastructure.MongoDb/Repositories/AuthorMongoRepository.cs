using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Репозиторий для работы с авторами в MongoDB
/// Предоставляет методы для CRUD-операций над документами авторов
/// </summary>
public class AuthorMongoRepository
{
    private readonly IMongoCollection<Author> _authors;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория авторов
    /// </summary>
    /// <param name="context">Контекст базы данных MongoDB</param>
    public AuthorMongoRepository(MongoDbContext context)
    {
        _authors = context.Authors;
    }

    /// <summary>
    /// Получить автора по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора</param>
    /// <returns>Модель автора или null если не найден</returns>
    public async Task<Author?> ReadAsync(int id)
    {
        return await _authors
            .Find(a => a.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Получить список всех авторов из базы данных
    /// </summary>
    /// <returns>Неизменяемый список всех авторов</returns>
    public async Task<IReadOnlyList<Author>> ReadAllAsync()
    {
        var result = await _authors
            .Find(FilterDefinition<Author>.Empty)
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать нового автора в базе данных
    /// </summary>
    /// <param name="entity">Модель автора для сохранения</param>
    /// <returns>Созданный автор с заполненными данными</returns>
    public async Task<Author> CreateAsync(Author entity)
    {
        await _authors.InsertOneAsync(entity);
        return entity;
    }

    /// <summary>
    /// Удалить автора из базы данных по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора</param>
    /// <returns>true если автор был удален, false если не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _authors.DeleteOneAsync(a => a.Id == id);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Обновить данные существующего автора
    /// </summary>
    /// <param name="entity">Модель автора с обновленными данными</param>
    /// <returns>Обновленный автор или null если не найден</returns>
    public async Task<Author?> UpdateAsync(Author entity)
    {
        var result = await _authors.ReplaceOneAsync(
            a => a.Id == entity.Id,
            entity);

        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}
