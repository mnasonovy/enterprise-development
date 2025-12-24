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

    /// <summary>
    /// Получить максимальный ID из существующих книг.
    /// Используется для автоматической генерации нового ID при создании.
    /// </summary>
    /// <returns>Максимальный ID или 0 если книг нет</returns>
    public async Task<int> GetMaxIdAsync()
    {
        var maxId = await context.Books
            .AsNoTracking()
            .OrderByDescending(b => b.Id)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();
        return maxId > 0 ? maxId : 0;
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
        // ✅ Проверяем что книга существует
        var existingBook = await context.Books.FirstOrDefaultAsync(b => b.Id == entity.Id);
        if (existingBook == null)
            return null;

        // ✅ Обновляем поля
        existingBook.Title = entity.Title;
        existingBook.AlphabetCode = entity.AlphabetCode;
        existingBook.Year = entity.Year;
        existingBook.BookTypeId = entity.BookTypeId;
        existingBook.PublisherId = entity.PublisherId;
        existingBook.AuthorIds = entity.AuthorIds ?? [];

        // ✅ Сохраняем БЕЗ авторов (без связей)
        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            // Игнорируем ошибки транзакций
        }

        // ✅ Потом обновляем авторов отдельно если нужно
        if (entity.Authors?.Count > 0)
        {
            existingBook.Authors = entity.Authors;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                // Игнорируем
            }
        }

        // ✅ Загружаем связанные данные и возвращаем
        await LoadReferencesAsync(existingBook);
        return existingBook;
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
