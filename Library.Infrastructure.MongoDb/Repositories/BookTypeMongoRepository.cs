using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

public class BookTypeMongoRepository
{
    private readonly IMongoCollection<BookType> _bookTypes;

    public BookTypeMongoRepository(MongoDbContext context)
    {
        _bookTypes = context.BookTypes;
    }

    public async Task<BookType?> ReadAsync(int id)
    {
        return await _bookTypes
            .Find(bt => bt.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BookType>> ReadAllAsync()
    {
        var result = await _bookTypes
            .Find(FilterDefinition<BookType>.Empty)
            .ToListAsync();
        return result;
    }

    public async Task<BookType> CreateAsync(BookType entity)
    {
        await _bookTypes.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _bookTypes.DeleteOneAsync(bt => bt.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<BookType?> UpdateAsync(BookType entity)
    {
        var result = await _bookTypes.ReplaceOneAsync(
            bt => bt.Id == entity.Id,
            entity);
        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}