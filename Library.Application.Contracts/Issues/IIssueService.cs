namespace Library.Application.Contracts.Issues;

/// <summary>
/// Интерфейс сервиса для управления выданными книгами.
/// Определяет контракт для работы с выданными книгами в приложении.
/// Все методы наследуются из базового интерфейса IApplicationService.
/// </summary>
public interface IIssueService
    : IApplicationService<IssueDto, IssueCreateUpdateDto, int>
{
    // Все CRUD методы наследуются от базового интерфейса:
    // - GetAsync(int id) → Task<IssueDto?>
    // - GetListAsync() → Task<IReadOnlyList<IssueDto>>
    // - CreateAsync(IssueCreateUpdateDto input) → Task<IssueDto>
    // - UpdateAsync(int id, IssueCreateUpdateDto input) → Task<IssueDto?>
    // - DeleteAsync(int id) → Task
}