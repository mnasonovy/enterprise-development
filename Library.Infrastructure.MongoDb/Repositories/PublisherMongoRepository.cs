using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

public class PublisherMongoRepository
{
    private readonly IMongoCollection<Publisher> _publishers;

    public PublisherMongoRepository(MongoDbContext context)
    {
        _publishers = context.Publishers;
    }

    public async Task<Publisher?> ReadAsync(int id)
    {
        return await _publishers
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Publisher>> ReadAllAsync()
    {
        var result = await _publishers
            .Find(FilterDefinition<Publisher>.Empty)
            .ToListAsync();
        return result;
    }

    public async Task<Publisher> CreateAsync(Publisher entity)
    {
        await _publishers.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _publishers.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Publisher?> UpdateAsync(Publisher entity)
    {
        var result = await _publishers.ReplaceOneAsync(
            p => p.Id == entity.Id,
            entity);
        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}