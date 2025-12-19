namespace Library.Application.Contracts.Publishers;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над издателями.
/// Определяет контракт для работы с издателями в приложении.
/// Все методы наследуются из базового интерфейса IApplicationService.
/// </summary>
public interface IPublisherService
    : IApplicationService<PublisherDto, PublisherCreateUpdateDto, int>
{
    // Все CRUD методы наследуются от базового интерфейса:
    // - GetAsync(int id) → Task<PublisherDto?>
    // - GetListAsync() → Task<IReadOnlyList<PublisherDto>>
    // - CreateAsync(PublisherCreateUpdateDto input) → Task<PublisherDto>
    // - UpdateAsync(int id, PublisherCreateUpdateDto input) → Task<PublisherDto?>
    // - DeleteAsync(int id) → Task
}
