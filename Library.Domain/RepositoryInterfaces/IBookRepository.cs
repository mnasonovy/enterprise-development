using Library.Domain.Models;

namespace Library.Domain.RepositoryInterfaces;

/// <summary>
/// Контракт репозитория для управления книгами в MongoDB.
/// Определяет асинхронные операции CRUD для сущности Book.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Получить книгу по идентификатору с загрузкой авторов.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>Объект Book с заполненными авторами или null если книга не найдена</returns>
    public Task<Book?> ReadAsync(int id);

    /// <summary>
    /// Получить список всех книг с загрузкой авторов.
    /// </summary>
    /// <returns>Неизменяемый список всех книг с заполненными авторами</returns>
    public Task<IReadOnlyList<Book>> ReadAllAsync();

    /// <summary>
    /// Создать новую книгу в базе данных.
    /// </summary>
    /// <param name="entity">Объект Book для сохранения</param>
    /// <returns>Созданный объект Book с заполненным Id и загруженными авторами</returns>
    public Task<Book> CreateAsync(Book entity);

    /// <summary>
    /// Обновить данные существующей книги.
    /// </summary>
    /// <param name="entity">Объект Book с обновленными данными</param>
    /// <returns>Обновленный объект Book с загруженными авторами или null если книга не найдена</returns>
    public Task<Book?> UpdateAsync(Book entity);

    /// <summary>
    /// Удалить книгу из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления</param>
    public Task DeleteAsync(int id);
}