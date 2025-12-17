using AutoMapper;
using Library.Application.Contracts.Authors;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над авторами.
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly AuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorService(AuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<AuthorDto?> GetAsync(int id)
    {
        var author = await _authorRepository.ReadAsync(id);
        return author == null ? null : _mapper.Map<AuthorDto>(author);
    }

    public async Task<IReadOnlyList<AuthorDto>> GetListAsync()
    {
        var authors = await _authorRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<AuthorDto>>(authors);
    }

    public async Task<AuthorDto> CreateAsync(AuthorDto input)
    {
        var author = _mapper.Map<Author>(input);
        var created = await _authorRepository.CreateAsync(author);
        return _mapper.Map<AuthorDto>(created);
    }

    public async Task<AuthorDto?> UpdateAsync(int id, AuthorDto input)
    {
        var existing = await _authorRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _authorRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<AuthorDto>(updated);
    }

    /// <summary>
    /// Удалить автора. 🔧 ИСПРАВЛЕНО: Task без возвращаемого типа!
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _authorRepository.DeleteAsync(id);
    }
}
