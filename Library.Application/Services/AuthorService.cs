using Library.Application.Contracts.Authors;
using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Repositories;

namespace Library.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly AuthorMongoRepository _authorRepository;

    public AuthorService(AuthorMongoRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto?> GetAsync(int id)
    {
        var author = await _authorRepository.ReadAsync(id);
        return author is null
            ? null
            : MapToDto(author);
    }

    public async Task<IReadOnlyList<AuthorDto>> GetListAsync()
    {
        var authors = await _authorRepository.ReadAllAsync();
        return authors.Select(MapToDto).ToArray();
    }

    public async Task<AuthorDto> CreateAsync(AuthorCreateUpdateDto input)
    {
        var author = new Author
        {
            Initials = input.Initials,
            LastName = input.LastName
        };

        var created = await _authorRepository.CreateAsync(author);
        return MapToDto(created);
    }

    public async Task<AuthorDto> UpdateAsync(int id, AuthorCreateUpdateDto input)
    {
        var existing = await _authorRepository.ReadAsync(id)
                       ?? throw new InvalidOperationException($"Author with id {id} was not found.");

        existing.Initials = input.Initials;
        existing.LastName = input.LastName;

        var updated = await _authorRepository.UpdateAsync(existing)
                      ?? throw new InvalidOperationException($"Author with id {id} was not updated.");

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await _authorRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Author with id {id} was not deleted.");
        }
    }

    private static AuthorDto MapToDto(Author author) => new()
    {
        Id = author.Id,
        Initials = author.Initials,
        LastName = author.LastName
    };
}
