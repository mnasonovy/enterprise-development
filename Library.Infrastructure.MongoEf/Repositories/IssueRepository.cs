using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с выданными книгами (Issue) через MongoDB EF Core.
/// Заменяет старый MongoDBDriver подход на современный EF Core.
/// 🔧 ИСПРАВЛЕНО: Используются РЕАЛЬНЫЕ свойства Issue!
/// </summary>
public class IssueRepository
{
    private readonly MongoDbContext _context;
    private readonly DbSet<Issue> _issues;

    public IssueRepository(MongoDbContext context)
    {
        _context = context;
        _issues = context.Issues;
    }

    /// <summary>
    /// Получить выданную книгу по идентификатору.
    /// 🔧 Include для Book и Reader (ОБЯЗАТЕЛЬНО!)
    /// </summary>
    public async Task<Issue?> ReadAsync(int id)
    {
        return await _issues
            .AsNoTracking()
            .Include(i => i.Book)
            .Include(i => i.Reader)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>
    /// Получить список всех выданных книг из базы данных.
    /// 🔧 Include для Book и Reader (ОБЯЗАТЕЛЬНО!)
    /// </summary>
    public async Task<IReadOnlyList<Issue>> ReadAllAsync()
    {
        var result = await _issues
            .AsNoTracking()
            .Include(i => i.Book)
            .Include(i => i.Reader)
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать новую выданную книгу в базе данных.
    /// </summary>
    public async Task<Issue> CreateAsync(Issue entity)
    {
        await _issues.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить выданную книгу из базы данных по идентификатору.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _issues.FirstOrDefaultAsync(i => i.Id == id);
        if (entity is null)
            return false;

        _issues.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Обновить данные существующей выданной книги.
    /// 🔧 ИСПРАВЛЕНО: Используются РЕАЛЬНЫЕ свойства IssueDate и ReturnDate!
    /// </summary>
    public async Task<Issue?> UpdateAsync(Issue entity)
    {
        // Загружаем существующую Issue с Book и Reader
        var existing = await _issues
            .Include(i => i.Book)
            .Include(i => i.Reader)
            .FirstOrDefaultAsync(i => i.Id == entity.Id);

        if (existing is null)
            return null;

        // 🔧 Обновляем РЕАЛЬНЫЕ свойства
        existing.IssueDate = entity.IssueDate;    // ✅ Правильно
        existing.DaysCount = entity.DaysCount;
        existing.ReturnDate = entity.ReturnDate;  // ✅ Правильно
        existing.BookId = entity.BookId;
        existing.ReaderId = entity.ReaderId;

        _issues.Update(existing);
        await _context.SaveChangesAsync();
        return existing;
    }
}
