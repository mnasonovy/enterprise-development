namespace Library.Application.Services;

using Library.Application.Contracts.Authors;

using Library.Domain.Models;

using Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Сервис для CRUD-операций над авторами
/// Преобразует Domain модели в DTOs и координирует операции с репозиторием
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly AuthorRepository _authorRepository;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса авторов
    /// </summary>
    /// <param name="authorRepository">Репозиторий для работы с MongoDB через EF Core</param>
    public AuthorService(AuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    /// <summary>
    /// Получить автора по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор автора</param>
    /// <returns>DTO автора или null если не найден</returns>
    public async Task<AuthorDto?> GetAsync(int id)
    {
        var author = await _authorRepository.ReadAsync(id);
        return author != null ? MapToDto(author) : null;
    }

    /// <summary>
    /// Получить список всех авторов
    /// </summary>
    /// <returns>Неизменяемый список DTO авторов</returns>
    public async Task<IReadOnlyList<AuthorDto>> GetListAsync()
    {
        var authors = await _authorRepository.ReadAllAsync();
        return authors.Select(MapToDto).ToList().AsReadOnly();
    }

    /// <summary>
    /// Создать нового автора
    /// </summary>
    /// <param name="input">DTO с данными нового автора</param>
    /// <returns>DTO созданного автора</returns>
    public async Task<AuthorDto> CreateAsync(AuthorDto input)
    {
        var author = new Author
        {
            Id = input.Id,
            Initials = input.Initials,
            LastName = input.LastName
        };

        var created = await _authorRepository.CreateAsync(author);
        return MapToDto(created);
    }

    /// <summary>
    /// Обновить данные автора
    /// </summary>
    /// <param name="id">Идентификатор автора</param>
    /// <param name="input">DTO с новыми данными</param>
    /// <returns>DTO обновленного автора или null если не найден</returns>
    public async Task<AuthorDto?> UpdateAsync(int id, AuthorDto input)
    {
        var existing = await _authorRepository.ReadAsync(id);
        if (existing == null)
            return null;

        existing.Initials = input.Initials;
        existing.LastName = input.LastName;

        var updated = await _authorRepository.UpdateAsync(existing);
        return updated != null ? MapToDto(updated) : null;
    }

    /// <summary>
    /// Удалить автора по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор автора</param>
    /// <returns>Задача удаления</returns>
    public async Task DeleteAsync(int id)
    {
        await _authorRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Преобразовать Domain модель автора в DTO
    /// </summary>
    /// <param name="author">Domain модель автора</param>
    /// <returns>DTO автора</returns>
    private static AuthorDto MapToDto(Author author)
    {
        return new()
        {
            Id = author.Id,
            Initials = author.Initials,
            LastName = author.LastName
        };
    }
}