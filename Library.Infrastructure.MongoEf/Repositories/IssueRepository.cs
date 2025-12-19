using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Репозиторий для работы с выданными книгами (Issue) через MongoDB EF Core.
/// Реализует интерфейс IIssueRepository и предоставляет методы для выполнения CRUD операций.
/// ВАЖНО: MongoDB EF Core НЕ поддерживает Include(), используем Entry().LoadAsync() вместо этого.
/// </summary>
public class IssueRepository(MongoDbContext context) : IIssueRepository
{
    private readonly DbSet<Issue> _issues = context.Issues;

    /// <summary>
    /// Получить выданную книгу по идентификатору.
    /// Загружает связанные сущности Book и Reader для полной информации.
    /// Использует Entry().LoadAsync() вместо Include(), так как MongoDB EF Core не поддерживает Include().
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи</param>
    /// <returns>Сущность Issue если найдена с загруженными Book и Reader; null если запись не существует</returns>
    public async Task<Issue?> GetAsync(int id)
    {
        var issue = await _issues.FirstOrDefaultAsync(i => i.Id == id);

        if (issue is null)
            return null;

        await LoadNavigationPropertiesAsync(issue);
        return issue;
    }

    /// <summary>
    /// Получить список всех выданных книг из базы данных.
    /// Загружает связанные сущности Book и Reader для каждой выдачи.
    /// Использует Entry().LoadAsync() вместо Include(), так как MongoDB EF Core не поддерживает Include().
    /// </summary>
    /// <returns>Неизменяемый список всех выданных книг с загруженными Book и Reader</returns>
    public async Task<IReadOnlyList<Issue>> GetListAsync()
    {
        var issues = await _issues.ToListAsync();

        // Загружаем Book и Reader для каждой Issue
        foreach (var issue in issues)
        {
            await LoadNavigationPropertiesAsync(issue);
        }

        return issues.AsReadOnly();
    }

    /// <summary>
    /// Создать новую выданную книгу в базе данных.
    /// Автоматически загружает связанные сущности Book и Reader для корректного маппинга в DTO.
    /// Использует Entry().LoadAsync() вместо Include().
    /// </summary>
    /// <param name="entity">Сущность Issue для сохранения</param>
    /// <returns>Созданная сущность Issue с заполненным идентификатором и загруженными связанными сущностями</returns>
    public async Task<Issue> CreateAsync(Issue entity)
    {
        await _issues.AddAsync(entity);
        await context.SaveChangesAsync();

        // Перезагружаем созданную Issue с Book и Reader для маппинга
        var createdIssue = await _issues.FirstOrDefaultAsync(i => i.Id == entity.Id);

        if (createdIssue is not null)
        {
            await LoadNavigationPropertiesAsync(createdIssue);
        }

        return createdIssue!;
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
        await context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Обновить данные существующей выданной книги.
    /// Загружает существующую запись с связанными сущностями и обновляет её параметры.
    /// Возвращает обновленную запись с загруженными Book и Reader для маппинга в DTO.
    /// Использует Entry().LoadAsync() вместо Include().
    /// </summary>
    /// <param name="entity">Сущность Issue с обновленными данными</param>
    /// <returns>Обновленная сущность Issue с загруженными связанными сущностями; null если запись не найдена</returns>
    public async Task<Issue?> UpdateAsync(Issue entity)
    {
        // Загружаем существующую Issue
        var existing = await _issues.FirstOrDefaultAsync(i => i.Id == entity.Id);

        if (existing is null)
            return null;

        // Обновляем свойства
        existing.IssueDate = entity.IssueDate;
        existing.DaysCount = entity.DaysCount;
        existing.ReturnDate = entity.ReturnDate;
        existing.BookId = entity.BookId;
        existing.ReaderId = entity.ReaderId;

        _issues.Update(existing);
        await context.SaveChangesAsync();

        // Перезагружаем обновленную Issue для гарантии загрузки Book и Reader
        var updatedIssue = await _issues.FirstOrDefaultAsync(i => i.Id == entity.Id);

        if (updatedIssue is not null)
        {
            await LoadNavigationPropertiesAsync(updatedIssue);
        }

        return updatedIssue;
    }

    /// <summary>
    /// Загрузить навигационные свойства (Book и Reader) для сущности Issue.
    /// Вспомогательный приватный метод для избежания дублирования кода.
    /// </summary>
    /// <param name="issue">Сущность Issue для загрузки навигаций</param>
    private async Task LoadNavigationPropertiesAsync(Issue issue)
    {
        await context.Entry(issue).Reference(i => i.Book).LoadAsync();
        await context.Entry(issue).Reference(i => i.Reader).LoadAsync();
    }
}