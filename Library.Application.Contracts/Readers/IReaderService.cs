namespace Library.Application.Contracts.Readers;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над читателями.
/// Определяет контракт для работы с читателями в приложении.
/// </summary>
public interface IReaderService : IApplicationService
{
    /// <summary>
    /// Получить читателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>DTO читателя или null если читатель не найден</returns>
    public Task<ReaderDto?> GetAsync(int id);

    /// <summary>
    /// Получить список всех читателей.
    /// </summary>
    /// <returns>Неизменяемый список DTO всех читателей</returns>
    public Task<IReadOnlyList<ReaderDto>> GetListAsync();

    /// <summary>
    /// Создать нового читателя в системе.
    /// </summary>
    /// <param name="input">DTO с данными нового читателя (FullName, Address, Phone, RegistrationDate)</param>
    /// <returns>DTO созданного читателя с заполненными данными</returns>
    public Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input);

    /// <summary>
    /// Обновить существующего читателя.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для обновления</param>
    /// <param name="input">DTO с новыми данными читателя</param>
    /// <returns>DTO обновленного читателя или null если читатель не найден</returns>
    public Task<ReaderDto?> UpdateAsync(int id, ReaderCreateUpdateDto input);

    /// <summary>
    /// Удалить читателя из системы.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    /// <returns>Задача удаления</returns>
    public Task DeleteAsync(int id);
}
