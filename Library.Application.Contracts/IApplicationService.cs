namespace Library.Application.Contracts;

/// <summary>
/// Базовый интерфейс для всех CRUD сервисов приложения.
/// Определяет контракт для асинхронного управления данными всех сущностей.
/// </summary>
/// <typeparam name="TDto">DTO для получения данных</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO для создания и обновления</typeparam>
/// <typeparam name="TKey">Тип идентификатора (обычно int)</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Получить сущность по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор</param>
    /// <returns>DTO сущности или null если не найдена</returns>
    public Task<TDto?> GetAsync(TKey id);

    /// <summary>
    /// Получить список всех сущностей.
    /// </summary>
    /// <returns>Неизменяемый список DTO всех сущностей</returns>
    public Task<IReadOnlyList<TDto>> GetListAsync();

    /// <summary>
    /// Создать новую сущность.
    /// </summary>
    /// <param name="input">DTO с данными для создания</param>
    /// <returns>DTO созданной сущности с ID</returns>
    public Task<TDto> CreateAsync(TCreateUpdateDto input);

    /// <summary>
    /// Обновить существующую сущность.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="input">DTO с новыми данными</param>
    /// <returns>DTO обновленной сущности или null если не найдена</returns>
    public Task<TDto?> UpdateAsync(TKey id, TCreateUpdateDto input);

    /// <summary>
    /// Удалить сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    public Task DeleteAsync(TKey id);

    /// <summary>
    /// Создать новую сущность или обновить существующую (Upsert).
    /// Идемпотентная операция, используется для синхронизации из NATS JetStream.
    /// </summary>
    /// <param name="input">DTO с данными сущности (должен содержать ID)</param>
    /// <returns>DTO сущности (созданной или обновленной)</returns>
    public Task<TDto> UpsertAsync(TCreateUpdateDto input);
}
