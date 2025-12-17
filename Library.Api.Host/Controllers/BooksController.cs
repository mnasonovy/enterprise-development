using Library.Application.Contracts.Books;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над книгами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(
    IBookService bookService,
    ILogger<BooksController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех книг
    /// GET: /api/books
    /// </summary>
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
    /// Получить книгу по идентификатору
    /// GET: /api/books/{id}
    /// </summary>
    [HttpGet("{id:int}")]  // ✅ Явно указываем :int
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
    /// Создать новую книгу
    /// POST: /api/books
    /// </summary>
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

        // ✅ ИСПРАВЛЕНО: Используем CreatedAtAction с правильной сигнатурой
        return CreatedAtAction(
            nameof(GetAsync),           // Имя метода для редиректа
            new { id = result.Id },     // Параметры для маршрута GetAsync
            result);                    // Тело ответа
    }

    /// <summary>
    /// Обновить данные книги
    /// PUT: /api/books/{id}
    /// </summary>
    [HttpPut("{id:int}")]  // ✅ Явно указываем :int
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
    /// Удалить книгу по идентификатору
    /// DELETE: /api/books/{id}
    /// </summary>
    [HttpDelete("{id:int}")]  // ✅ Явно указываем :int
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
