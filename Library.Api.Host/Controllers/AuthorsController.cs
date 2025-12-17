using Library.Application.Contracts.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для управления авторами.
/// Предоставляет REST API endpoints для выполнения CRUD операций над авторами.
/// Все методы асинхронные с полной валидацией входных данных и логированием.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список всех авторов.
    /// HTTP GET: /api/authors
    /// Возвращает 200 с полным списком авторов, 204 если авторов нет.
    /// </summary>
    /// <returns>Коллекция DTO всех авторов или пустой список.</returns>
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
    /// Получает автора по уникальному идентификатору.
    /// HTTP GET: /api/authors/{id}
    /// Возвращает 200 с данными автора, 404 если автор не найден.
    /// </summary>
    /// <param name="id">Идентификатор автора для поиска.</param>
    /// <returns>DTO автора, если найден.</returns>
    [HttpGet("{id:int}")]
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
    /// Создаёт нового автора в системе.
    /// HTTP POST: /api/authors
    /// Требует валидное имя (фамилия не пусто и не только пробелы).
    /// ID должен быть установлен вручную и быть больше 0.
    /// Возвращает 201 с созданным автором и заголовком Location.
    /// </summary>
    /// <param name="input">DTO с данными нового автора (LastName и Id обязательны).</param>
    /// <returns>DTO созданного автора с назначенным идентификатором.</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] AuthorCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Author data is required");

        if (input.Id <= 0)
            return BadRequest("Author ID must be set manually and be greater than 0");

        if (string.IsNullOrWhiteSpace(input.LastName))
            return BadRequest("Author last name is required");

        try
        {
            var result = await authorService.CreateAsync(input);

            logger.LogInformation("{Method} method executed successfully with id = {Id}",
                nameof(CreateAsync), result.Id);

            return Created($"/api/authors/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error creating Author: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновляет информацию об существующем авторе.
    /// HTTP PUT: /api/authors/{id}
    /// Возвращает 200 с обновленными данными, 404 если автор не найден.
    /// </summary>
    /// <param name="id">Идентификатор автора для обновления.</param>
    /// <param name="input">DTO с новыми данными автора.</param>
    /// <returns>Обновленный DTO автора.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("Author data is required");

        if (string.IsNullOrWhiteSpace(input.LastName))
            return BadRequest("Author last name is required");

        try
        {
            var result = await authorService.UpdateAsync(id, input);

            logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

            return result != null ? Ok(result) : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error updating Author: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удаляет автора из системы по идентификатору.
    /// HTTP DELETE: /api/authors/{id}
    /// Возвращает 204 при успешном удалении.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(DeleteAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        try
        {
            await authorService.DeleteAsync(id);

            logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error deleting Author: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
