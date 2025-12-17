using Library.Application.Contracts.BookTypes;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над типами книг
/// 🔧 ИСПРАВЛЕНО: DELETE return statement, логирование, валидация
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookTypesController(
    IBookTypeService bookTypeService,
    ILogger<BookTypesController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех типов книг
    /// GET: /api/booktypes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookTypeDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));

        var result = await bookTypeService.GetListAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);

        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить тип книги по идентификатору
    /// GET: /api/booktypes/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookTypeDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        var result = await bookTypeService.GetAsync(id);

        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать новый тип книги
    /// POST: /api/booktypes
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BookTypeDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] BookTypeCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("BookType data is required");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("BookType name is required");

        var result = await bookTypeService.CreateAsync(input);

        logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), result.Id);

        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Обновить данные типа книги
    /// PUT: /api/booktypes/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookTypeDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookTypeCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("BookType data is required");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("BookType name is required");

        var result = await bookTypeService.UpdateAsync(id, input);

        logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Удалить тип книги по идентификатору
    /// DELETE: /api/booktypes/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(DeleteAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        await bookTypeService.DeleteAsync(id);

        logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

        return NoContent(); // 🔧 ДОБАВЛЕНО return!
    }
}
