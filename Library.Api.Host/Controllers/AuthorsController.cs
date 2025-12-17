using Library.Application.Contracts.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над авторами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController(
    IAuthorService authorService,
    ILogger<AuthorsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех авторов
    /// GET: /api/authors
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<AuthorDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));

        var result = await authorService.GetListAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);

        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить автора по идентификатору
    /// GET: /api/authors/{id}
    /// </summary>
    [HttpGet("{id:int}")]  // ✅ Явно указываем :int
    [ProducesResponseType(200, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        var result = await authorService.GetAsync(id);

        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать нового автора
    /// POST: /api/authors
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] AuthorDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Author data is required");

        if (string.IsNullOrWhiteSpace(input.LastName))
            return BadRequest("Author last name is required");

        var result = await authorService.CreateAsync(input);

        logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), result.Id);

        // ✅ ИСПРАВЛЕНО: Используем CreatedAtAction с правильной сигнатурой
        return CreatedAtAction(
            nameof(GetAsync),           // Имя метода для редиректа
            new { id = result.Id },     // Параметры для маршрута GetAsync
            result);                    // Тело ответа
    }

    /// <summary>
    /// Обновить данные автора
    /// PUT: /api/authors/{id}
    /// </summary>
    [HttpPut("{id:int}")]  // ✅ Явно указываем :int
    [ProducesResponseType(200, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("Author data is required");

        if (string.IsNullOrWhiteSpace(input.LastName))
            return BadRequest("Author last name is required");

        var result = await authorService.UpdateAsync(id, input);

        logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Удалить автора по идентификатору
    /// DELETE: /api/authors/{id}
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

        await authorService.DeleteAsync(id);

        logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

        return NoContent();
    }
}
