using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

public class AuthorMongoRepository
{
    private readonly IMongoCollection<Author> _authors;

    public AuthorMongoRepository(MongoDbContext context)
    {
        _authors = context.Authors;
    }

    public async Task<Author?> ReadAsync(int id)
    {
        return await _authors
            .Find(a => a.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Author>> ReadAllAsync()
    {
        var result = await _authors
            .Find(FilterDefinition<Author>.Empty)
            .ToListAsync();

        return result;
    }

    public async Task<Author> CreateAsync(Author entity)
    {
        await _authors.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _authors.DeleteOneAsync(a => a.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Author?> UpdateAsync(Author entity)
    {
        var result = await _authors.ReplaceOneAsync(
            a => a.Id == entity.Id,
            entity);

        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}
