namespace Library.Application.Contracts.Publishers;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над издателями.
/// Определяет контракт для работы с издателями в приложении.
/// </summary>
public interface IPublisherService : IApplicationService
{
    /// <summary>
    /// Получить издателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>DTO издателя или null если издатель не найден</returns>
    public Task<PublisherDto?> GetAsync(int id);

    /// <summary>
    /// Получить список всех издателей.
    /// </summary>
    /// <returns>Неизменяемый список DTO всех издателей</returns>
    public Task<IReadOnlyList<PublisherDto>> GetListAsync();

    /// <summary>
    /// Создать нового издателя в системе.
    /// ID должен быть установлен вручную и быть больше 0.
    /// </summary>
    /// <param name="input">DTO с данными нового издателя (Name и Id обязательны)</param>
    /// <returns>DTO созданного издателя с заполненными данными</returns>
    public Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input);

    /// <summary>
    /// Обновить существующего издателя.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для обновления</param>
    /// <param name="input">DTO с новыми данными издателя</param>
    /// <returns>DTO обновленного издателя или null если издатель не найден</returns>
    public Task<PublisherDto?> UpdateAsync(int id, PublisherCreateUpdateDto input);

    /// <summary>
    /// Удалить издателя из системы.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для удаления</param>
    public Task DeleteAsync(int id);
}
