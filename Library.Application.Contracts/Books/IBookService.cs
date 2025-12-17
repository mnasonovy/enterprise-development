namespace Library.Application.Contracts.Books;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над книгами.
/// Определяет контракт для работы с книгами в приложении.
/// </summary>
public interface IBookService : IApplicationService
{
    /// <summary>
    /// Получить книгу по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>DTO книги или null если книга не найдена</returns>
    public Task<BookDto?> GetAsync(int id);

    /// <summary>
    /// Получить список всех книг.
    /// </summary>
    /// <returns>Неизменяемый список DTO всех книг</returns>
    public Task<IReadOnlyList<BookDto>> GetListAsync();

    /// <summary>
    /// Создать новую книгу в системе.
    /// </summary>
    /// <param name="input">DTO с данными новой книги (Title, Year, BookTypeId, PublisherId, AuthorIds)</param>
    /// <returns>DTO созданной книги с заполненными данными</returns>
    public Task<BookDto> CreateAsync(BookCreateUpdateDto input);

    /// <summary>
    /// Обновить существующую книгу.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для обновления</param>
    /// <param name="input">DTO с новыми данными книги</param>
    /// <returns>DTO обновленной книги или null если книга не найдена</returns>
    public Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input);

    /// <summary>
    /// Удалить книгу из системы.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления</param>
    /// <returns>Задача удаления</returns>
    public Task DeleteAsync(int id);
}
