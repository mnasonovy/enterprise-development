namespace Library.Application.Contracts.Readers;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над читателями.
/// Определяет контракт для работы с читателями в приложении.
/// Все методы наследуются из базового интерфейса IApplicationService.
/// </summary>
public interface IReaderService
    : IApplicationService<ReaderDto, ReaderCreateUpdateDto, int>
{
    // Все CRUD методы наследуются от базового интерфейса:
    // - GetAsync(int id) → Task<ReaderDto?>
    // - GetListAsync() → Task<IReadOnlyList<ReaderDto>>
    // - CreateAsync(ReaderCreateUpdateDto input) → Task<ReaderDto>
    // - UpdateAsync(int id, ReaderCreateUpdateDto input) → Task<ReaderDto?>
    // - DeleteAsync(int id) → Task
}
