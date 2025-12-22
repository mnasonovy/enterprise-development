using Library.Domain.Models;

namespace Library.Domain.RepositoryInterfaces;

/// <summary>
/// Контракт (интерфейс) для репозитория Author.
/// Определяет все методы которые должны быть реализованы для работы с авторами.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Получает автора по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для поиска.</param>
    /// <returns>Модель Author, если найдена; null если автор не существует.</returns>
    public Task<Author?> ReadAsync(int id);

    /// <summary>
    /// Получает список всех авторов из базы данных.
    /// </summary>
    /// <returns>Коллекция всех авторов. Если авторов нет, возвращает пустой список.</returns>
    public Task<IReadOnlyList<Author>> ReadAllAsync();

    /// <summary>
    /// Создаёт нового автора в базе данных.
    /// </summary>
    /// <param name="entity">Модель Author с данными нового автора.</param>
    /// <returns>Созданный Author с назначенным идентификатором.</returns>
    public Task<Author> CreateAsync(Author entity);

    /// <summary>
    /// Обновляет информацию об существующем авторе.
    /// </summary>
    /// <param name="entity">Модель Author с обновленными данными.</param>
    /// <returns>Обновленный Author, если успешно; null если автор не найден.</returns>
    public Task<Author?> UpdateAsync(Author entity);

    /// <summary>
    /// Удаляет автора из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    /// <returns>true если автор был успешно удален; false если автор не найден.</returns>
    public Task<bool> DeleteAsync(int id);
}