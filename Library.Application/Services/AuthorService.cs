using AutoMapper;
using Library.Application.Contracts.Authors;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления авторами.
/// Реализует интерфейс IAuthorService, обеспечивая выполнение CRUD операций над авторами.
/// Использует AutoMapper для преобразования между Domain моделями и DTO.
/// </summary>
public class AuthorService(IAuthorRepository authorRepository, IMapper mapper) : IAuthorService
{
    /// <summary>
    /// Получает автора по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для поиска.</param>
    /// <returns>DTO автора, если найден; null если автор не существует.</returns>
    public async Task<AuthorDto?> GetAsync(int id)
    {
        var author = await authorRepository.ReadAsync(id);
        return author == null ? null : mapper.Map<AuthorDto>(author);
    }

    /// <summary>
    /// Получает список всех авторов.
    /// </summary>
    /// <returns>Коллекция DTO всех авторов. Если авторов нет, возвращает пустой список.</returns>
    public async Task<IReadOnlyList<AuthorDto>> GetListAsync()
    {
        var authors = await authorRepository.ReadAllAsync();
        return mapper.Map<IReadOnlyList<AuthorDto>>(authors);
    }

    /// <summary>
    /// Создаёт нового автора в базе данных.
    /// </summary>
    /// <param name="input">DTO с данными нового автора.</param>
    /// <returns>DTO созданного автора с назначенным идентификатором.</returns>
    public async Task<AuthorDto> CreateAsync(AuthorCreateUpdateDto input)
    {
        var author = mapper.Map<Author>(input);
        var created = await authorRepository.CreateAsync(author);
        return mapper.Map<AuthorDto>(created);
    }

    /// <summary>
    /// Обновляет информацию об существующем авторе.
    /// </summary>
    /// <param name="id">Идентификатор автора для обновления.</param>
    /// <param name="input">DTO с новыми данными автора.</param>
    /// <returns>Обновленный DTO автора, если успешно; null если автор не найден.</returns>
    public async Task<AuthorDto?> UpdateAsync(int id, AuthorCreateUpdateDto input)
    {
        var existing = await authorRepository.ReadAsync(id);
        if (existing == null)
            return null;

        mapper.Map(input, existing);
        var updated = await authorRepository.UpdateAsync(existing);
        return updated == null ? null : mapper.Map<AuthorDto>(updated);
    }

    /// <summary>
    /// Удаляет автора из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    public async Task DeleteAsync(int id)
    {
        await authorRepository.DeleteAsync(id);
    }
}