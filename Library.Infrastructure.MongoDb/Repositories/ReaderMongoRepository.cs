using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

public class ReaderMongoRepository
{
    private readonly IMongoCollection<Reader> _readers;

    public ReaderMongoRepository(MongoDbContext context)
    {
        _readers = context.Readers;
    }

    public async Task<Reader?> ReadAsync(int id)
    {
        return await _readers
            .Find(r => r.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Reader>> ReadAllAsync()
    {
        var result = await _readers
            .Find(FilterDefinition<Reader>.Empty)
            .ToListAsync();

        return result;
    }

    public async Task<Reader> CreateAsync(Reader entity)
    {
        await _readers.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _readers.DeleteOneAsync(r => r.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Reader?> UpdateAsync(Reader entity)
    {
        var result = await _readers.ReplaceOneAsync(
            r => r.Id == entity.Id,
            entity);

        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}
