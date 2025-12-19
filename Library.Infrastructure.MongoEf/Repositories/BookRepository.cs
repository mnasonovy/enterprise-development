using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с книгами через MongoDB EF Core.
/// Загружает тип книги, издателя и авторов по их Id.
/// </summary>
public class BookRepository(MongoDbContext context) : IBookRepository
{
    private readonly DbSet<Book> _books = context.Books;
    private readonly DbSet<Author> _authors = context.Authors;
    private readonly DbSet<BookType> _bookTypes = context.BookTypes;
    private readonly DbSet<Publisher> _publishers = context.Publishers;

    public async Task<Book?> ReadAsync(int id)
    {
        var book = await _books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book != null)
            await LoadReferencesAsync(book);

        return book;
    }

    public async Task<IReadOnlyList<Book>> ReadAllAsync()
    {
        var books = await _books
            .AsNoTracking()
            .ToListAsync();

        foreach (var book in books)
            await LoadReferencesAsync(book);

        return books.AsReadOnly();
    }

    public async Task<Book> CreateAsync(Book entity)
    {
        await _books.AddAsync(entity);
        await context.SaveChangesAsync();

        await LoadReferencesAsync(entity);

        return entity;
    }

    public async Task<Book?> UpdateAsync(Book entity)
    {
        var exists = await _books.AnyAsync(b => b.Id == entity.Id);
        if (!exists)
            return null;

        _books.Update(entity);
        await context.SaveChangesAsync();

        await LoadReferencesAsync(entity);

        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _books.FirstOrDefaultAsync(b => b.Id == id);
        if (entity == null)
            return;

        _books.Remove(entity);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Подтягивает BookType, Publisher и Authors по их Id (без Include).
    /// </summary>
    private async Task LoadReferencesAsync(Book book)
    {
        // Авторы
        if (book.AuthorIds == null || book.AuthorIds.Count == 0)
        {
            book.Authors = [];
        }
        else
        {
            var authors = await _authors
                .AsNoTracking()
                .Where(a => book.AuthorIds.Contains(a.Id))
                .ToListAsync();

            book.Authors = authors;
        }

        // Тип книги
        book.BookType = await _bookTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == book.BookTypeId)
            ?? throw new KeyNotFoundException($"Тип книги с ID {book.BookTypeId} не найден");

        // Издатель
        book.Publisher = await _publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == book.PublisherId)
            ?? throw new KeyNotFoundException($"Издатель с ID {book.PublisherId} не найден");
    }
}
