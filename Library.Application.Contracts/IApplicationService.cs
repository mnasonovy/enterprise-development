namespace Library.Application.Contracts;

/// <summary>
/// Базовый generic интерфейс для всех CRUD сервисов приложения.
/// Определяет контракт для асинхронного управления данными.
/// </summary>
/// <typeparam name="TDto">DTO для получения/вывода данных</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO для создания и обновления</typeparam>
/// <typeparam name="TKey">Тип идентификатора (обычно int)</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Получить один элемент по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>DTO или null если не найден</returns>
    public Task<TDto?> GetAsync(TKey id);

    /// <summary>
    /// Получить полный список всех элементов.
    /// </summary>
    /// <returns>Неизменяемый список DTO</returns>
    public Task<IReadOnlyList<TDto>> GetListAsync();

    /// <summary>
    /// Создать новый элемент.
    /// </summary>
    /// <param name="input">DTO для создания</param>
    /// <returns>Созданный DTO с ID</returns>
    public Task<TDto> CreateAsync(TCreateUpdateDto input);

    /// <summary>
    /// Обновить существующий элемент.
    /// </summary>
    /// <param name="id">Идентификатор элемента</param>
    /// <param name="input">DTO с новыми данными</param>
    /// <returns>Обновленный DTO или null если не найден</returns>
    public Task<TDto?> UpdateAsync(TKey id, TCreateUpdateDto input);

    /// <summary>
    /// Удалить элемент по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор</param>
    public Task DeleteAsync(TKey id);
}
