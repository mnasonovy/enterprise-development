using Library.Domain.Models;

using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Library.Infrastructure.MongoEf.Database;

/// <summary>
/// MongoDB EF Core контекст для работы с LibraryDb
/// Предоставляет DbSet для всех сущностей
/// </summary>
public class MongoDbContext : DbContext
{
    public MongoDbContext(DbContextOptions<MongoDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet для работы с книгами
    /// </summary>
    public DbSet<Book> Books { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с авторами
    /// </summary>
    public DbSet<Author> Authors { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с читателями
    /// </summary>
    public DbSet<Reader> Readers { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с проблемами/изданиями
    /// </summary>
    public DbSet<Issue> Issues { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с издателями
    /// </summary>
    public DbSet<Publisher> Publishers { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с типами книг
    /// </summary>
    public DbSet<BookType> BookTypes { get; set; } = null!;

    /// <summary>
    /// Конфигурация модели данных для MongoDB
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация для MongoDB коллекций
        // Используем имена коллекций в нижнем регистре
        modelBuilder.Entity<Book>().ToCollection("books");
        modelBuilder.Entity<Author>().ToCollection("authors");
        modelBuilder.Entity<Reader>().ToCollection("readers");
        modelBuilder.Entity<Issue>().ToCollection("issues");
        modelBuilder.Entity<Publisher>().ToCollection("publishers");
        modelBuilder.Entity<BookType>().ToCollection("booktypes");
    }
}