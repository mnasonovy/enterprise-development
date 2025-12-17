using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Library.Infrastructure.MongoEf.Database;

/// <summary>
/// MongoDB EF Core контекст для работы с LibraryDb.
/// Предоставляет DbSet для всех сущностей и конфигурирует отношения между ними.
/// </summary>
public class MongoDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр MongoDbContext с переданными параметрами.
    /// </summary>
    public MongoDbContext(DbContextOptions<MongoDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet для работы с книгами.
    /// </summary>
    public DbSet<Book> Books { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с авторами.
    /// </summary>
    public DbSet<Author> Authors { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с читателями.
    /// </summary>
    public DbSet<Reader> Readers { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с выданными книгами (выдачи).
    /// </summary>
    public DbSet<Issue> Issues { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с издателями.
    /// </summary>
    public DbSet<Publisher> Publishers { get; set; } = null!;

    /// <summary>
    /// DbSet для работы с типами книг.
    /// </summary>
    public DbSet<BookType> BookTypes { get; set; } = null!;

    /// <summary>
    /// Конфигурация модели данных для MongoDB.
    /// Определяет коллекции, первичные ключи и отношения между сущностями.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ════════════════════════════════════════════════════════════════
        // КОЛЛЕКЦИИ MONGODB - Имена коллекций в нижнем регистре
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<Book>().ToCollection("books");
        modelBuilder.Entity<Author>().ToCollection("authors");
        modelBuilder.Entity<Reader>().ToCollection("readers");
        modelBuilder.Entity<Issue>().ToCollection("issues");
        modelBuilder.Entity<Publisher>().ToCollection("publishers");
        modelBuilder.Entity<BookType>().ToCollection("booktypes");

        // ════════════════════════════════════════════════════════════════
        // КОНФИГУРАЦИЯ: BOOK (Главная сущность)
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);

        // Book (1) ──→ (М) BookType
        modelBuilder.Entity<Book>()
            .HasOne(b => b.BookType)
            .WithMany(bt => bt.Books)
            .HasForeignKey(b => b.BookTypeId)
            .IsRequired();

        // Book (1) ──→ (М) Publisher
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .IsRequired();

        // Book (1) ──→ (М) Issue
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Issues)
            .WithOne(i => i.Book)
            .HasForeignKey(i => i.BookId)
            .IsRequired();

        // Book ↔ (М-М) Author (многие-ко-многим)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity(
                "BookAuthors",
                l => l.HasOne(typeof(Author)).WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.Cascade),
                r => r.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.Cascade),
                j => j.ToCollection("book_authors"));

        // ════════════════════════════════════════════════════════════════
        // КОНФИГУРАЦИЯ: AUTHOR
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<Author>()
            .HasKey(a => a.Id);

        // ════════════════════════════════════════════════════════════════
        // КОНФИГУРАЦИЯ: READER (Читатель библиотеки)
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<Reader>()
            .HasKey(r => r.Id);

        // Reader (1) ──→ (М) Issue
        modelBuilder.Entity<Reader>()
            .HasMany(r => r.Issues)
            .WithOne(i => i.Reader)
            .HasForeignKey(i => i.ReaderId)
            .IsRequired();

        // ════════════════════════════════════════════════════════════════
        // КОНФИГУРАЦИЯ: ISSUE (ВЫДАЧА КНИГИ)
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<Issue>()
            .HasKey(i => i.Id);

        // Issue (М) ──→ (1) Book
        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Book)
            .WithMany(b => b.Issues)
            .HasForeignKey(i => i.BookId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Issue (М) ──→ (1) Reader
        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Reader)
            .WithMany(r => r.Issues)
            .HasForeignKey(i => i.ReaderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // ════════════════════════════════════════════════════════════════
        // КОНФИГУРАЦИЯ: PUBLISHER (Издатель)
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<Publisher>()
            .HasKey(p => p.Id);

        // Publisher (1) ──→ (М) Book
        modelBuilder.Entity<Publisher>()
            .HasMany(p => p.Books)
            .WithOne(b => b.Publisher)
            .HasForeignKey(b => b.PublisherId)
            .IsRequired();

        // ════════════════════════════════════════════════════════════════
        // КОНФИГУРАЦИЯ: BOOKTYPE (Тип книги)
        // ════════════════════════════════════════════════════════════════

        modelBuilder.Entity<BookType>()
            .HasKey(bt => bt.Id);

        // BookType (1) ──→ (М) Book
        modelBuilder.Entity<BookType>()
            .HasMany(bt => bt.Books)
            .WithOne(b => b.BookType)
            .HasForeignKey(b => b.BookTypeId)
            .IsRequired();
    }
}
