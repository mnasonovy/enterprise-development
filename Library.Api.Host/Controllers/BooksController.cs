using Library.Application.Contracts.Books;

using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления книгами в библиотеке.
/// Предоставляет endpoints для выполнения CRUD-операций над книгами.
/// Маршруты: GET, POST, PUT, DELETE на /api/books.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(
    IBookService bookService,
    ILogger<BooksController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех книг из каталога библиотеки.
    /// Возвращает 200 OK с коллекцией книг или 204 No Content если нет книг.
    /// </summary>
    /// <returns>Коллекция BookDto или пустой результат.</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));
        var result = await bookService.GetListAsync();
        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);

        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить информацию о конкретной книге по её ID.
    /// Возвращает 200 OK если найдена или 404 Not Found если не существует.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги (должен быть > 0).</param>
    /// <returns>BookDto если найдена, иначе 404.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        var result = await bookService.GetAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать новую книгу в каталоге библиотеки.
    /// Валидирует все обязательные поля и возвращает 201 Created.
    /// </summary>
    /// <param name="input">DTO с данными новой книги (Title, Year, BookTypeId, PublisherId, AuthorIds).</param>
    /// <returns>201 Created с BookDto и Location header, или 400 Bad Request при ошибке валидации.</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] BookCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Book data is required");

        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest("Book title is required");

        if (input.BookTypeId <= 0)
            return BadRequest("Valid book type id is required");

        if (input.PublisherId <= 0)
            return BadRequest("Valid publisher id is required");

        if (input.AuthorIds == null || input.AuthorIds.Count == 0)
            return BadRequest("At least one author id is required");

        if (input.AuthorIds.Any(id => id <= 0))
            return BadRequest("All author ids must be greater than 0");

        var result = await bookService.CreateAsync(input);
        logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), result.Id);

        // Возвращаем 201 Created с Location header для новой книги
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Обновить информацию о существующей книге.
    /// Валидирует все поля и возвращает 200 OK с обновленными данными.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для обновления (должен быть > 0).</param>
    /// <param name="input">DTO с новыми данными книги.</param>
    /// <returns>200 OK с обновленным BookDto, 404 Not Found если книга не существует, или 400 Bad Request при ошибке.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("Book data is required");

        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest("Book title is required");

        if (input.BookTypeId <= 0)
            return BadRequest("Valid book type id is required");

        if (input.PublisherId <= 0)
            return BadRequest("Valid publisher id is required");

        if (input.AuthorIds == null || input.AuthorIds.Count == 0)
            return BadRequest("At least one author id is required");

        if (input.AuthorIds.Any(authorId => authorId <= 0))
            return BadRequest("All author ids must be greater than 0");

        var result = await bookService.UpdateAsync(id, input);
        logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Удалить книгу из каталога библиотеки по её ID.
    /// Возвращает 204 No Content при успехе.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления (должен быть > 0).</param>
    /// <returns>204 No Content при успехе, или 400 Bad Request при неверном ID.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(DeleteAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        await bookService.DeleteAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

        return NoContent();
    }
}
