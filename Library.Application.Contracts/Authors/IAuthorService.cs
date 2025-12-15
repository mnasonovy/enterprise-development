namespace Library.Application.Contracts.Authors;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над авторами
/// Определяет контракт для работы с авторами в приложении
/// </summary>
public interface IAuthorService : IApplicationService
{
    /// <summary>
    /// Получить автора по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора</param>
    /// <returns>DTO автора или null если автор не найден</returns>
    public Task<AuthorDto?> GetAsync(int id);

    /// <summary>
    /// Получить список всех авторов
    /// </summary>
    /// <returns>Неизменяемый список DTO всех авторов</returns>
    public Task<IReadOnlyList<AuthorDto>> GetListAsync();

    /// <summary>
    /// Создать нового автора в системе
    /// </summary>
    /// <param name="input">DTO с данными нового автора (должен содержать Id, LastName)</param>
    /// <returns>DTO созданного автора с заполненными данными</returns>
    public Task<AuthorDto> CreateAsync(AuthorDto input);

    /// <summary>
    /// Обновить существующего автора
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора для обновления</param>
    /// <param name="input">DTO с новыми данными автора</param>
    /// <returns>DTO обновленного автора или null если автор не найден</returns>
    public Task<AuthorDto?> UpdateAsync(int id, AuthorDto input);

    /// <summary>
    /// Удалить автора из системы
    /// </summary>
    /// <param name="id">Уникальный идентификатор автора для удаления</param>
    /// <returns>Задача удаления</returns>
    public Task DeleteAsync(int id);
}
