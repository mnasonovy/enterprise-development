namespace Library.Application.Contracts.Books;

/// <summary>
/// Интерфейс приложения для управления книгами в библиотеке.
/// Определяет контракт для выполнения CRUD-операций над книгами.
/// </summary>
public interface IBookService : IApplicationService
{
    /// <summary>
    /// Получить информацию о книге по её идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги в базе данных.</param>
    /// <returns>DTO книги или null, если книга с указанным ID не найдена.</returns>
    public Task<BookDto?> GetAsync(int id);

    /// <summary>
    /// Получить полный список всех книг в библиотеке.
    /// </summary>
    /// <returns>Неизменяемая коллекция DTO всех книг в каталоге.</returns>
    public Task<IReadOnlyList<BookDto>> GetListAsync();

    /// <summary>
    /// Создать новую книгу в каталоге библиотеки.
    /// </summary>
    /// <param name="input">DTO с данными новой книги: Title, Year, BookTypeId, PublisherId, AuthorIds.</param>
    /// <returns>DTO созданной книги с автоматически заполненным ID и связанными данными.</returns>
    public Task<BookDto> CreateAsync(BookCreateUpdateDto input);

    /// <summary>
    /// Обновить информацию о существующей книге.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для обновления.</param>
    /// <param name="input">DTO с новыми данными книги.</param>
    /// <returns>DTO обновленной книги или null, если книга с указанным ID не найдена.</returns>
    public Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input);

    /// <summary>
    /// Удалить книгу из каталога библиотеки.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления.</param>
    /// <returns>Асинхронная задача удаления.</returns>
    public Task DeleteAsync(int id);
}
