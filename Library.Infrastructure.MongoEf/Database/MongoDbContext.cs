using Library.Domain.Models;

using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Library.Infrastructure.MongoEf.Database;

/// <summary>
/// Контекст базы данных MongoDB для приложения библиотеки.
/// </summary>
public class MongoDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Reader> Readers { get; set; } = null!;
    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<BookType> BookTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>().ToCollection("books");
        modelBuilder.Entity<Author>().ToCollection("authors");
        modelBuilder.Entity<Reader>().ToCollection("readers");
        modelBuilder.Entity<Issue>().ToCollection("issues");
        modelBuilder.Entity<Publisher>().ToCollection("publishers");
        modelBuilder.Entity<BookType>().ToCollection("booktypes");

        // ========================================
        // BOOK
        // ========================================
        modelBuilder.Entity<Book>().HasKey(b => b.Id);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.BookType)
            .WithMany(bt => bt.Books)
            .HasForeignKey(b => b.BookTypeId)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Issues)
            .WithOne(i => i.Book)
            .HasForeignKey(i => i.BookId)
            .IsRequired();

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
        // AUTHOR
        // ========================================
        modelBuilder.Entity<Author>().HasKey(a => a.Id);

        // ========================================
        // READER
        // ========================================
        modelBuilder.Entity<Reader>().HasKey(r => r.Id);

        modelBuilder.Entity<Reader>()
            .HasMany(r => r.Issues)
            .WithOne(i => i.Reader)
            .HasForeignKey(i => i.ReaderId)
            .IsRequired();

        // ========================================
        // ISSUE - БЕЗ RESTRICT!
        // ========================================
        modelBuilder.Entity<Issue>().HasKey(i => i.Id);

        // Issue → Book (Cascade - чтоб не было конфликтов)
        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Book)
            .WithMany(b => b.Issues)
            .HasForeignKey(i => i.BookId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Issue → Reader (Cascade)
        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Reader)
            .WithMany(r => r.Issues)
            .HasForeignKey(i => i.ReaderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // ========================================
        // PUBLISHER
        // ========================================
        modelBuilder.Entity<Publisher>().HasKey(p => p.Id);

        modelBuilder.Entity<Publisher>()
            .HasMany(p => p.Books)
            .WithOne(b => b.Publisher)
            .HasForeignKey(b => b.PublisherId)
            .IsRequired();

        // ========================================
        // BOOKTYPE
        // ========================================
        modelBuilder.Entity<BookType>().HasKey(bt => bt.Id);

        modelBuilder.Entity<BookType>()
            .HasMany(bt => bt.Books)
            .WithOne(b => b.BookType)
            .HasForeignKey(b => b.BookTypeId)
            .IsRequired();
    }
}
