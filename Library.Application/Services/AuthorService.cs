using AutoMapper;
using Library.Application.Contracts.Authors;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления авторами в библиотечной системе.
/// Реализует CRUD операции и Upsert для синхронизации с NATS JetStream.
/// </summary>
public class AuthorService(IAuthorRepository authorRepository, IMapper mapper) : IAuthorService
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получает автора по идентификатору.
    /// </summary>
    public async Task<AuthorDto?> GetAsync(int id)
    {
        var author = await _authorRepository.ReadAsync(id);
        return author == null ? null : _mapper.Map<AuthorDto>(author);
    }

    /// <summary>
    /// Получает список всех авторов.
    /// </summary>
    public async Task<IReadOnlyList<AuthorDto>> GetListAsync()
    {
        var authors = await _authorRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<AuthorDto>>(authors);
    }

    /// <summary>
    /// Создаёт нового автора.
    /// </summary>
    public async Task<AuthorDto> CreateAsync(AuthorCreateUpdateDto input)
    {
        var author = _mapper.Map<Author>(input);
        var created = await _authorRepository.CreateAsync(author);
        return _mapper.Map<AuthorDto>(created);
    }

    /// <summary>
    /// Обновляет существующего автора.
    /// </summary>
    public async Task<AuthorDto?> UpdateAsync(int id, AuthorCreateUpdateDto input)
    {
        var existing = await _authorRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _authorRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<AuthorDto>(updated);
    }

    /// <summary>
    /// Удаляет автора по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _authorRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Создаёт нового автора или обновляет существующего (Upsert).
    /// Идемпотентная операция для синхронизации из NATS.
    /// </summary>
    public async Task<AuthorDto> UpsertAsync(AuthorCreateUpdateDto input)
    {
        var existing = await _authorRepository.ReadAsync(input.Id);

        if (existing != null)
        {
            _mapper.Map(input, existing);
            var updated = await _authorRepository.UpdateAsync(existing);
            return _mapper.Map<AuthorDto>(updated)!;
        }
        else
        {
            var author = _mapper.Map<Author>(input);
            var created = await _authorRepository.CreateAsync(author);
            return _mapper.Map<AuthorDto>(created);
        }
    }
}
