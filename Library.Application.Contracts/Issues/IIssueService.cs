namespace Library.Application.Contracts.Issues;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над выданными книгами (выдачи).
/// Определяет контракт для работы с записями о выданных книгах.
/// </summary>
public interface IIssueService : IApplicationService
{
    /// <summary>
    /// Получить запись о выдаче по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи</param>
    /// <returns>DTO выдачи или null если запись не найдена</returns>
    public Task<IssueDto?> GetAsync(int id);

    /// <summary>
    /// Получить список всех выданных книг.
    /// </summary>
    /// <returns>Неизменяемый список DTO всех выдач</returns>
    public Task<IReadOnlyList<IssueDto>> GetListAsync();

    /// <summary>
    /// Создать новую запись о выдаче книги.
    /// </summary>
    /// <param name="input">DTO с данными выдачи (BookId, ReaderId, IssueDate, DaysCount)</param>
    /// <returns>DTO созданной выдачи с заполненными данными</returns>
    public Task<IssueDto> CreateAsync(IssueCreateUpdateDto input);

    /// <summary>
    /// Обновить существующую запись о выдаче.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для обновления</param>
    /// <param name="input">DTO с новыми данными выдачи</param>
    /// <returns>DTO обновленной выдачи или null если запись не найдена</returns>
    public Task<IssueDto?> UpdateAsync(int id, IssueCreateUpdateDto input);

    /// <summary>
    /// Удалить запись о выдаче из системы.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для удаления</param>
    /// <returns>Задача удаления</returns>
    public Task DeleteAsync(int id);
}
