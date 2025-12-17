namespace Library.Application.Contracts.Authors;

/// <summary>
/// Интерфейс сервиса для управления авторами.
/// Определяет контракт для выполнения CRUD операций над авторами в системе.
/// </summary>
public interface IAuthorService : IApplicationService
{
    /// <summary>
    /// Получает автора по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для поиска.</param>
    /// <returns>DTO автора, если найден; null если автор не существует.</returns>
    public Task<AuthorDto?> GetAsync(int id);

    /// <summary>
    /// Получает список всех авторов из базы данных.
    /// </summary>
    /// <returns>Коллекция DTO всех авторов. Если авторов нет, возвращает пустой список.</returns>
    public Task<IReadOnlyList<AuthorDto>> GetListAsync();

    /// <summary>
    /// Создаёт нового автора в базе данных.
    /// </summary>
    /// <param name="input">DTO с данными нового автора (фамилия обязательна).</param>
    /// <returns>DTO созданного автора с назначенным идентификатором.</returns>
    public Task<AuthorDto> CreateAsync(AuthorDto input);

    /// <summary>
    /// Обновляет информацию об существующем авторе.
    /// </summary>
    /// <param name="id">Идентификатор автора для обновления.</param>
    /// <param name="input">DTO с новыми данными автора.</param>
    /// <returns>Обновленный DTO автора, если успешно; null если автор не найден.</returns>
    public Task<AuthorDto?> UpdateAsync(int id, AuthorDto input);

    /// <summary>
    /// Удаляет автора из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    public Task DeleteAsync(int id);
}