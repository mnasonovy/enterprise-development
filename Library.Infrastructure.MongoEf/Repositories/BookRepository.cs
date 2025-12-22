using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с книгами через MongoDB EF Core.
/// Загружает тип книги, издателя и авторов по их Id.
/// </summary>
public class BookRepository(MongoDbContext context) : IBookRepository
{
    public async Task<Book?> ReadAsync(int id)
    {
        var book = await context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book != null)
            await LoadReferencesAsync(book);

        return book;
    }

    public async Task<IReadOnlyList<Book>> ReadAllAsync()
    {
        var books = await context.Books
            .AsNoTracking()
            .ToListAsync();

        foreach (var book in books)
            await LoadReferencesAsync(book);

        return books.AsReadOnly();
    }

    public async Task<Book> CreateAsync(Book entity)
    {
        await context.Books.AddAsync(entity);
        await context.SaveChangesAsync();

        await LoadReferencesAsync(entity);

        return entity;
    }

    public async Task<Book?> UpdateAsync(Book entity)
    {
        var exists = await context.Books.AnyAsync(b => b.Id == entity.Id);
        if (!exists)
            return null;

        context.Books.Update(entity);
        await context.SaveChangesAsync();

        await LoadReferencesAsync(entity);

        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (entity == null)
            return;

        context.Books.Remove(entity);
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
            var authors = await context.Authors
                .AsNoTracking()
                .Where(a => book.AuthorIds.Contains(a.Id))
                .ToListAsync();

            book.Authors = authors;
        }

        // Тип книги
        book.BookType = await context.BookTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == book.BookTypeId)
            ?? throw new KeyNotFoundException($"Тип книги с ID {book.BookTypeId} не найден");

        // Издатель
        book.Publisher = await context.Publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == book.PublisherId)
            ?? throw new KeyNotFoundException($"Издатель с ID {book.PublisherId} не найден");
    }
}
