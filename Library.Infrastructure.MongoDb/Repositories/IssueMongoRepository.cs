using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

public class IssueMongoRepository
{
    private readonly IMongoCollection<Issue> _issues;

    public IssueMongoRepository(MongoDbContext context)
    {
        _issues = context.Issues;
    }

    public async Task<Issue?> ReadAsync(int id)
    {
        return await _issues
            .Find(i => i.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Issue>> ReadAllAsync()
    {
        var result = await _issues
            .Find(FilterDefinition<Issue>.Empty)
            .ToListAsync();

        return result;
    }

    public async Task<Issue> CreateAsync(Issue entity)
    {
        await _issues.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _issues.DeleteOneAsync(i => i.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Issue?> UpdateAsync(Issue entity)
    {
        var result = await _issues.ReplaceOneAsync(
            i => i.Id == entity.Id,
            entity);

        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}
