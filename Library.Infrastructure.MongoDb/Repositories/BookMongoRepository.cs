using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Database;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Repositories;

public class BookMongoRepository
{
    private readonly IMongoCollection<Book> _books;

    public BookMongoRepository(MongoDbContext context)
    {
        _books = context.Books;
    }

    public async Task<Book?> ReadAsync(int id)
    {
        return await _books
            .Find(b => b.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Book>> ReadAllAsync()
    {
        var result = await _books
            .Find(FilterDefinition<Book>.Empty)
            .ToListAsync();

        return result;
    }

    public async Task<Book> CreateAsync(Book entity)
    {
        // простая схема: Id задаётся вручную (позже можно сделать автоинкремент)
        await _books.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _books.DeleteOneAsync(b => b.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Book?> UpdateAsync(Book entity)
    {
        var result = await _books.ReplaceOneAsync(
            b => b.Id == entity.Id,
            entity);

        if (result.MatchedCount == 0)
        {
            return null;
        }

        return entity;
    }
}
