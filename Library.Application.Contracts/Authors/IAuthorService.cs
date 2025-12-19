namespace Library.Application.Contracts.Authors;

/// <summary>
/// Интерфейс сервиса для управления авторами.
/// Определяет контракт для выполнения CRUD операций над авторами в системе.
/// Наследует все методы из базового интерфейса IApplicationService.
/// </summary>
public interface IAuthorService
    : IApplicationService<AuthorDto, AuthorCreateUpdateDto, int>
{
    // Все CRUD методы наследуются от базового интерфейса:
    // - GetAsync(int id) → Task<AuthorDto?>
    // - GetListAsync() → Task<IReadOnlyList<AuthorDto>>
    // - CreateAsync(AuthorCreateUpdateDto input) → Task<AuthorDto>
    // - UpdateAsync(int id, AuthorCreateUpdateDto input) → Task<AuthorDto?>
    // - DeleteAsync(int id) → Task
}
