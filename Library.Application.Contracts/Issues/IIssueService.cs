namespace Library.Application.Contracts.Issues;

/// <summary>
/// Интерфейс сервиса для управления выданными книгами.
/// Определяет CRUD операции.
/// </summary>
public interface IIssueService : IApplicationService
{
    /// <summary>
    /// Получить выдачу по ID.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи</param>
    /// <returns>DTO выдачи или null если не найдена</returns>
    public Task<IssueDto?> GetAsync(int id);

    /// <summary>
    /// Получить все выданные книги.
    /// </summary>
    /// <returns>Список всех выдач</returns>
    public Task<IReadOnlyList<IssueDto>> GetListAsync();

    /// <summary>
    /// Создать новую выдачу книги.
    /// </summary>
    /// <param name="input">Данные выдачи (BookId, ReaderId, IssueDate, DaysCount)</param>
    /// <returns>DTO созданной выдачи</returns>
    public Task<IssueDto> CreateAsync(IssueCreateUpdateDto input);

    /// <summary>
    /// Обновить выдачу (для отметки возврата).
    /// </summary>
    /// <param name="id">ID выдачи для обновления</param>
    /// <param name="input">Новые данные выдачи</param>
    /// <returns>DTO обновленной выдачи или null если не найдена</returns>
    public Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input);

    /// <summary>
    /// Удалить выдачу из системы.
    /// </summary>
    /// <param name="id">ID выдачи для удаления</param>
    public Task DeleteAsync(int id);
}
