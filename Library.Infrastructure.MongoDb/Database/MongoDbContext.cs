using Library.Domain.Models;
using MongoDB.Driver;

namespace Library.Infrastructure.MongoDb.Database;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Book> Books => _database.GetCollection<Book>("books");
    public IMongoCollection<Author> Authors => _database.GetCollection<Author>("authors");
    public IMongoCollection<Reader> Readers => _database.GetCollection<Reader>("readers");
    public IMongoCollection<Issue> Issues => _database.GetCollection<Issue>("issues");
    public IMongoCollection<Publisher> Publishers => _database.GetCollection<Publisher>("publishers");
    public IMongoCollection<BookType> BookTypes => _database.GetCollection<BookType>("booktypes");
}