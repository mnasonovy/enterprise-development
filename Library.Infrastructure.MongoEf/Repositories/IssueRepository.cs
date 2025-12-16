using System.Collections.Generic;
using System.Threading.Tasks;

using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Database;

using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с проблемами через MongoDB EF Core
/// Заменяет старый MongoDBDriver подход на современный EF Core
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
    /// Получить проблему по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор проблемы</param>
    /// <returns>Модель проблемы или null если не найдена</returns>
    public async Task<Issue?> ReadAsync(int id)
    {
        return await _issues
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>
    /// Получить список всех проблем из базы данных
    /// </summary>
    /// <returns>Список всех проблем</returns>
    public async Task<IReadOnlyList<Issue>> ReadAllAsync()
    {
        var result = await _issues
            .AsNoTracking()
            .ToListAsync();

        return result.AsReadOnly();
    }

    /// <summary>
    /// Создать новую проблему в базе данных
    /// </summary>
    /// <param name="entity">Модель проблемы для сохранения</param>
    /// <returns>Созданная проблема с заполненными данными</returns>
    public async Task<Issue> CreateAsync(Issue entity)
    {
        await _issues.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Удалить проблему из базы данных по идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор проблемы</param>
    /// <returns>true если проблема была удалена, false если не найдена</returns>
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
    /// Обновить данные существующей проблемы
    /// </summary>
    /// <param name="entity">Модель проблемы с обновленными данными</param>
    /// <returns>Обновленная проблема или null если не найдена</returns>
    public async Task<Issue?> UpdateAsync(Issue entity)
    {
        var exists = await _issues.AnyAsync(i => i.Id == entity.Id);

        if (!exists)
            return null;

        _issues.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}