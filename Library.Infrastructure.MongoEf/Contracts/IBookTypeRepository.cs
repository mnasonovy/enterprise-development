using Library.Domain.Models;

namespace Library.Infrastructure.MongoEf.Contracts;

/// <summary>
/// Контракт репозитория для управления типами книг в MongoDB.
/// Определяет асинхронные операции CRUD для сущности BookType.
/// </summary>
public interface IBookTypeRepository
{
    /// <summary>
    /// Получить тип книги по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>Объект BookType или null если тип не найден</returns>
    public Task<BookType?> ReadAsync(int id);

    /// <summary>
    /// Получить список всех типов книг из базы данных.
    /// </summary>
    /// <returns>Неизменяемый список всех типов книг</returns>
    public Task<IReadOnlyList<BookType>> ReadAllAsync();

    /// <summary>
    /// Создать новый тип книги в базе данных.
    /// </summary>
    /// <param name="entity">Объект BookType для сохранения</param>
    /// <returns>Созданный объект BookType с заполненным Id</returns>
    public Task<BookType> CreateAsync(BookType entity);

    /// <summary>
    /// Обновить данные существующего типа книги.
    /// </summary>
    /// <param name="entity">Объект BookType с обновленными данными</param>
    /// <returns>Обновленный объект BookType или null если тип не найден</returns>
    public Task<BookType?> UpdateAsync(BookType entity);

    /// <summary>
    /// Удалить тип книги из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги для удаления</param>
    /// <returns>true если удаление успешно, false если тип не найден</returns>
    public Task<bool> DeleteAsync(int id);
}
