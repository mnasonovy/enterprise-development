using Library.Domain.Models;

namespace Library.Domain.RepositoryInterfaces;

/// <summary>
/// Контракт репозитория для управления читателями в MongoDB.
/// Определяет асинхронные операции CRUD для сущности Reader.
/// </summary>
public interface IReaderRepository
{
    /// <summary>
    /// Получить читателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>Объект Reader или null если читатель не найден</returns>
    public Task<Reader?> ReadAsync(int id);

    /// <summary>
    /// Получить список всех читателей из базы данных.
    /// </summary>
    /// <returns>Неизменяемый список всех читателей</returns>
    public Task<IReadOnlyList<Reader>> ReadAllAsync();

    /// <summary>
    /// Получить максимальный ID из существующих читателей.
    /// Используется для автоматической генерации следующего ID.
    /// </summary>
    /// <returns>Максимальный ID или 0 если читателей нет</returns>
    public Task<int> GetMaxIdAsync();

    /// <summary>
    /// Создать нового читателя в базе данных.
    /// </summary>
    /// <param name="entity">Объект Reader для сохранения</param>
    /// <returns>Созданный объект Reader с заполненным Id</returns>
    public Task<Reader> CreateAsync(Reader entity);

    /// <summary>
    /// Обновить данные существующего читателя.
    /// </summary>
    /// <param name="entity">Объект Reader с обновленными данными</param>
    /// <returns>Обновленный объект Reader или null если читатель не найден</returns>
    public Task<Reader?> UpdateAsync(Reader entity);

    /// <summary>
    /// Удалить читателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    /// <returns>true если удаление успешно, false если читатель не найден</returns>
    public Task<bool> DeleteAsync(int id);
}
