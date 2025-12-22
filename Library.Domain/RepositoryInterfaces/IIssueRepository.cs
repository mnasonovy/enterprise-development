using Library.Domain.Models;

namespace Library.Domain.RepositoryInterfaces;

/// <summary>
/// Контракт репозитория для работы с выданными книгами (Issue).
/// Определяет методы для выполнения CRUD операций над сущностью Issue.
/// </summary>
public interface IIssueRepository
{
    /// <summary>
    /// Получить выданную книгу по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выданной книги</param>
    /// <returns>Объект Issue если найден; null если не существует</returns>
    public Task<Issue?> GetAsync(int id);

    /// <summary>
    /// Получить список всех выданных книг.
    /// </summary>
    /// <returns>Неизменяемый список всех выданных книг</returns>
    public Task<IReadOnlyList<Issue>> GetListAsync();

    /// <summary>
    /// Создать новую выданную книгу.
    /// </summary>
    /// <param name="entity">Объект Issue для сохранения</param>
    /// <returns>Созданный объект Issue с заполненным Id</returns>
    public Task<Issue> CreateAsync(Issue entity);

    /// <summary>
    /// Обновить существующую выданную книгу.
    /// </summary>
    /// <param name="entity">Объект Issue с обновленными данными</param>
    /// <returns>Обновленный объект Issue или null если не найден</returns>
    public Task<Issue?> UpdateAsync(Issue entity);

    /// <summary>
    /// Удалить выданную книгу по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор для удаления</param>
    /// <returns>true если успешно удалена; false если запись не найдена</returns>
    public Task<bool> DeleteAsync(int id);
}