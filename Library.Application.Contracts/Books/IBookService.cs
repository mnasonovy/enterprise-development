namespace Library.Application.Contracts.Books;

/// <summary>
/// Интерфейс приложения для управления книгами в библиотеке.
/// Определяет контракт для выполнения CRUD-операций над книгами.
/// Все методы наследуются из базового интерфейса IApplicationService.
/// </summary>
public interface IBookService
    : IApplicationService<BookDto, BookCreateUpdateDto, int>
{
    // Все CRUD методы наследуются от базового интерфейса:
    // - GetAsync(int id) → Task<BookDto?>
    // - GetListAsync() → Task<IReadOnlyList<BookDto>>
    // - CreateAsync(BookCreateUpdateDto input) → Task<BookDto>
    // - UpdateAsync(int id, BookCreateUpdateDto input) → Task<BookDto?>
    // - DeleteAsync(int id) → Task
}
