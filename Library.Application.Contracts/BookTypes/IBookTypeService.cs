namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// Интерфейс сервиса для CRUD-операций над типами книг.
/// Определяет контракт для работы с типами книг в приложении.
/// Все методы наследуются из базового интерфейса IApplicationService.
/// </summary>
public interface IBookTypeService
    : IApplicationService<BookTypeDto, BookTypeCreateUpdateDto, int>
{
    // Все CRUD методы наследуются от базового интерфейса:
    // - GetAsync(int id) → Task<BookTypeDto?>
    // - GetListAsync() → Task<IReadOnlyList<BookTypeDto>>
    // - CreateAsync(BookTypeCreateUpdateDto input) → Task<BookTypeDto>
    // - UpdateAsync(int id, BookTypeCreateUpdateDto input) → Task<BookTypeDto?>
    // - DeleteAsync(int id) → Task
}
