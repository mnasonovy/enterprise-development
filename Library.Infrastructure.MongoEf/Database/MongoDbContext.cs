using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Library.Infrastructure.MongoEf.Database;

/// <summary>
/// Контекст базы данных MongoDB для приложения библиотеки.
/// Управляет всеми сущностями (книги, авторы, читатели, выпуски, издатели, типы книг)
/// и определяет их отношения в MongoDB.
/// </summary>
public class MongoDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="MongoDbContext"/>.
    /// </summary>
    /// <param name="options">Опции конфигурации контекста</param>
    public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Набор данных для коллекции книг.
    /// Содержит все книги в каталоге библиотеки.
    /// </summary>
    public DbSet<Book> Books { get; set; } = null!;

    /// <summary>
    /// Набор данных для коллекции авторов.
    /// Содержит информацию обо всех авторах книг в каталоге.
    /// </summary>
    public DbSet<Author> Authors { get; set; } = null!;

    /// <summary>
    /// Набор данных для коллекции читателей.
    /// Содержит информацию обо всех зарегистрированных читателях библиотеки.
    /// </summary>
    public DbSet<Reader> Readers { get; set; } = null!;

    /// <summary>
    /// Набор данных для коллекции выпусков.
    /// Содержит информацию о всех выпусках (экземплярах) книг в библиотеке.
    /// </summary>
    public DbSet<Issue> Issues { get; set; } = null!;

    /// <summary>
    /// Набор данных для коллекции издателей.
    /// Содержит информацию обо всех издателях книг в каталоге.
    /// </summary>
    public DbSet<Publisher> Publishers { get; set; } = null!;

    /// <summary>
    /// Набор данных для коллекции типов книг.
    /// Содержит справочник категорий книг (роман, учебник, справочник и т.д.).
    /// </summary>
    public DbSet<BookType> BookTypes { get; set; } = null!;

    /// <summary>
    /// Конфигурирует модель данных и отношения между сущностями в MongoDB.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели для конфигурации сущностей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Определяет имена коллекций MongoDB для каждой сущности
        modelBuilder.Entity<Book>().ToCollection("books");
        modelBuilder.Entity<Author>().ToCollection("authors");
        modelBuilder.Entity<Reader>().ToCollection("readers");
        modelBuilder.Entity<Issue>().ToCollection("issues");
        modelBuilder.Entity<Publisher>().ToCollection("publishers");
        modelBuilder.Entity<BookType>().ToCollection("booktypes");

        // ========================================
        // КОНФИГУРАЦИЯ СУЩНОСТИ BOOK
        // ========================================
        modelBuilder.Entity<Book>().HasKey(b => b.Id);

        // Связь Book → BookType (многие-к-одному)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.BookType)
            .WithMany(bt => bt.Books)
            .HasForeignKey(b => b.BookTypeId)
            .IsRequired();

        // Связь Book → Publisher (многие-к-одному)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .IsRequired();

        // Связь Book → Issue (один-ко-многим)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Issues)
            .WithOne(i => i.Book)
            .HasForeignKey(i => i.BookId)
            .IsRequired();

        // Связь Book ↔ Author (многие-ко-многим)
        // Используется коллекция book_authors для хранения связей
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity("BookAuthors",
                l => l.HasOne(typeof(Author))
                    .WithMany()
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.Cascade),
                r => r.HasOne(typeof(Book))
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.ToCollection("book_authors"));

        // ========================================
        // КОНФИГУРАЦИЯ СУЩНОСТИ AUTHOR
        // ========================================
        modelBuilder.Entity<Author>().HasKey(a => a.Id);

        // ========================================
        // КОНФИГУРАЦИЯ СУЩНОСТИ READER
        // ========================================
        modelBuilder.Entity<Reader>().HasKey(r => r.Id);

        // Связь Reader → Issue (один-ко-многим)
        modelBuilder.Entity<Reader>()
            .HasMany(r => r.Issues)
            .WithOne(i => i.Reader)
            .HasForeignKey(i => i.ReaderId)
            .IsRequired();

        // ========================================
        // КОНФИГУРАЦИЯ СУЩНОСТИ ISSUE
        // ========================================
        modelBuilder.Entity<Issue>().HasKey(i => i.Id);

        // Связь Issue → Book (многие-к-одному)
        // При удалении книги выпуск ограничивается (Restrict)
        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Book)
            .WithMany(b => b.Issues)
            .HasForeignKey(i => i.BookId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Связь Issue → Reader (многие-к-одному)
        // При удалении читателя выпуск ограничивается (Restrict)
        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Reader)
            .WithMany(r => r.Issues)
            .HasForeignKey(i => i.ReaderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // ========================================
        // КОНФИГУРАЦИЯ СУЩНОСТИ PUBLISHER
        // ========================================
        modelBuilder.Entity<Publisher>().HasKey(p => p.Id);

        // Связь Publisher → Book (один-ко-многим)
        modelBuilder.Entity<Publisher>()
            .HasMany(p => p.Books)
            .WithOne(b => b.Publisher)
            .HasForeignKey(b => b.PublisherId)
            .IsRequired();

        // ========================================
        // КОНФИГУРАЦИЯ СУЩНОСТИ BOOKTYPE
        // ========================================
        modelBuilder.Entity<BookType>().HasKey(bt => bt.Id);

        // Связь BookType → Book (один-ко-многим)
        modelBuilder.Entity<BookType>()
            .HasMany(bt => bt.Books)
            .WithOne(b => b.BookType)
            .HasForeignKey(b => b.BookTypeId)
            .IsRequired();
    }
}
