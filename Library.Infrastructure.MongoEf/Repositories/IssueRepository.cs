using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с выданными книгами (Issue) через MongoDB EF Core.
/// Предоставляет методы для выполнения CRUD операций над сущностью Issue.
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
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи</param>
    /// <returns>Сущность Issue если найдена; null если запись не существует</returns>
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
    /// </summary>
    /// <returns>Неизменяемый список всех выданных книг с загруженными Book и Reader</returns>
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
    /// <param name="entity">Сущность Issue для сохранения</param>
    /// <returns>Созданная сущность Issue с заполненным идентификатором</returns>
    public async Task<Issue> CreateAsync(Issue entity)
    {
        await _issues.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удалить выданную книгу из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для удаления</param>
    /// <returns>true если выданная книга успешно удалена; false если запись не найдена</returns>
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
    /// </summary>
    /// <param name="entity">Сущность Issue с обновленными данными</param>
    /// <returns>Обновленная сущность Issue; null если запись не найдена</returns>
    public async Task<Issue?> UpdateAsync(Issue entity)
    {
        // Загружаем существующую Issue с Book и Reader
        var existing = await _issues
            .Include(i => i.Book)
            .Include(i => i.Reader)
            .FirstOrDefaultAsync(i => i.Id == entity.Id);
        if (existing is null)
            return null;

        // Обновляем свойства
        existing.IssueDate = entity.IssueDate;
        existing.DaysCount = entity.DaysCount;
        existing.ReturnDate = entity.ReturnDate;
        existing.BookId = entity.BookId;
        existing.ReaderId = entity.ReaderId;

        _issues.Update(existing);
        await _context.SaveChangesAsync();
        return existing;
    }
}
