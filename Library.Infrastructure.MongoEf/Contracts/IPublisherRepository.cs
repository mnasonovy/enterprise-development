using Library.Domain.Models;

namespace Library.Infrastructure.MongoEf.Contracts;

/// <summary>
/// Контракт репозитория для управления издателями в MongoDB.
/// Определяет асинхронные операции CRUD для сущности Publisher.
/// </summary>
public interface IPublisherRepository
{
    /// <summary>
    /// Получить издателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>Объект Publisher или null если издатель не найден</returns>
    public Task<Publisher?> ReadAsync(int id);

    /// <summary>
    /// Получить список всех издателей из базы данных.
    /// </summary>
    /// <returns>Неизменяемый список всех издателей</returns>
    public Task<IReadOnlyList<Publisher>> ReadAllAsync();

    /// <summary>
    /// Создать нового издателя в базе данных.
    /// </summary>
    /// <param name="entity">Объект Publisher для сохранения</param>
    /// <returns>Созданный объект Publisher с заполненным Id</returns>
    public Task<Publisher> CreateAsync(Publisher entity);

    /// <summary>
    /// Обновить данные существующего издателя.
    /// </summary>
    /// <param name="entity">Объект Publisher с обновленными данными</param>
    /// <returns>Обновленный объект Publisher или null если издатель не найден</returns>
    public Task<Publisher?> UpdateAsync(Publisher entity);

    /// <summary>
    /// Удалить издателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для удаления</param>
    /// <returns>true если удаление успешно, false если издатель не найден</returns>
    public Task<bool> DeleteAsync(int id);
}
