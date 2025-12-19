using Library.Application.Contracts.Books;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления книгами в каталоге библиотеки.
/// Реализует полный набор CRUD-операций: получение списка, получение по ID,
/// создание новой книги, обновление и удаление.
/// 
/// Обработка ошибок перенесена в глобальный ExceptionHandlingMiddleware:
/// - Валидация входных данных → ArgumentException / ArgumentNullException (400)
/// - Запрошенный ресурс не найден → KeyNotFoundException (404)
/// - Конфликты / бизнес-ошибки → InvalidOperationException (409)
/// - Все остальные ошибки → 500 Internal Server Error
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService, ILogger<BooksController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список всех книг из каталога.
    /// HTTP GET: /api/books
    /// Возвращает 200 с полным списком книг, 204 если каталог пуст.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetListAsync));

        var result = await bookService.GetListAsync();

        if (result.Count == 0)
        {
            logger.LogInformation("{Method} executed: no books found (204)", nameof(GetListAsync));
            return NoContent(); // 204
        }

        logger.LogInformation("{Method} executed successfully with {Count} items (200)",
            nameof(GetListAsync), result.Count);

        return Ok(result); // 200
    }

    /// <summary>
    /// Получает информацию о книге по идентификатору.
    /// HTTP GET: /api/books/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(GetAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        var result = await bookService.GetAsync(id);

        if (result == null)
            throw new KeyNotFoundException($"Книга с ID {id} не найдена");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(GetAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Создает новую книгу в каталоге.
    /// HTTP POST: /api/books
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] BookCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called", nameof(CreateAsync));

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (input.Id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {input.Id}");

        if (string.IsNullOrWhiteSpace(input.Title))
            throw new ArgumentException("Название книги (Title) не может быть пустым");

        if (input.Year <= 0)
            throw new ArgumentException($"Год издания (Year) должен быть больше 0, получено: {input.Year}");

        if (input.BookTypeId <= 0)
            throw new ArgumentException($"BookTypeId должен быть больше 0, получено: {input.BookTypeId}");

        if (input.PublisherId <= 0)
            throw new ArgumentException($"PublisherId должен быть больше 0, получено: {input.PublisherId}");

        var result = await bookService.CreateAsync(input);

        logger.LogInformation("{Method} executed successfully with id={Id} (201)",
            nameof(CreateAsync), result.Id);

        return Created($"/api/books/{result.Id}", result); // 201
    }

    /// <summary>
    /// Обновляет существующую книгу в каталоге.
    /// HTTP PUT: /api/books/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (string.IsNullOrWhiteSpace(input.Title))
            throw new ArgumentException("Название книги (Title) не может быть пустым");

        if (input.Year <= 0)
            throw new ArgumentException($"Год издания (Year) должен быть больше 0, получено: {input.Year}");

        if (input.BookTypeId <= 0)
            throw new ArgumentException($"BookTypeId должен быть больше 0, получено: {input.BookTypeId}");

        if (input.PublisherId <= 0)
            throw new ArgumentException($"PublisherId должен быть больше 0, получено: {input.PublisherId}");

        var result = await bookService.UpdateAsync(id, input);

        if (result == null)
            throw new KeyNotFoundException($"Книга с ID {id} не найдена для обновления");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(UpdateAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Удаляет книгу из каталога по идентификатору.
    /// HTTP DELETE: /api/books/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(DeleteAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        try
        {
            await bookService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            // Пробрасываем дальше, чтобы middleware вернул 404
            throw new KeyNotFoundException($"Книга с ID {id} не найдена для удаления");
        }

        logger.LogInformation("{Method} executed successfully with id={Id} (204)",
            nameof(DeleteAsync), id);

        return NoContent(); // 204
    }
}
