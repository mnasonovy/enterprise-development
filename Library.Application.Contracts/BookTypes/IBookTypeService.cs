namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над типами книг.
/// Определяет контракт для работы с типами книг в приложении.
/// Все методы асинхронные и возвращают Task.
/// </summary>
public interface IBookTypeService : IApplicationService
{
    /// <summary>
    /// Получить тип книги по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>DTO типа книги или null если тип не найден</returns>
    public Task<BookTypeDto?> GetAsync(int id);

    /// <summary>
    /// Получить список всех типов книг.
    /// </summary>
    /// <returns>Неизменяемый список DTO всех типов книг</returns>
    public Task<IReadOnlyList<BookTypeDto>> GetListAsync();

    /// <summary>
    /// Создать новый тип книги в системе.
    /// </summary>
    /// <param name="input">DTO с данными нового типа книги (Name)</param>
    /// <returns>DTO созданного типа книги с заполненными данными</returns>
    public Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input);

    /// <summary>
    /// Обновить существующий тип книги.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги для обновления</param>
    /// <param name="input">DTO с новыми данными типа книги</param>
    /// <returns>DTO обновленного типа книги или null если тип не найден</returns>
    public Task<BookTypeDto?> UpdateAsync(int id, BookTypeCreateUpdateDto input);

    /// <summary>
    /// Удалить тип книги из системы.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги для удаления</param>
    /// <returns>Задача удаления</returns>
    public Task DeleteAsync(int id);
}
