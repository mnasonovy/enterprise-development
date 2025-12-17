using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для управления читателями.
/// Предоставляет REST API endpoints для выполнения CRUD операций над читателями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReadersController(
    IReaderService readerService,
    ILogger<ReadersController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех читателей.
    /// GET: /api/readers
    /// </summary>
    /// <returns>Список всех читателей или 204 No Content если список пуст</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<ReaderDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));
        var result = await readerService.GetListAsync();
        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);
        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить читателя по идентификатору.
    /// GET: /api/readers/{id}
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>Читатель если найден, иначе 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        var result = await readerService.GetAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать нового читателя.
    /// POST: /api/readers
    /// </summary>
    /// <param name="input">DTO с данными нового читателя (FullName и RegistrationDate обязательны)</param>
    /// <returns>Созданный читатель с кодом 201 Created</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] ReaderCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));
        if (input == null)
            return BadRequest("Reader data is required");
        if (string.IsNullOrWhiteSpace(input.FullName))
            return BadRequest("Reader full name is required");
        if (input.RegistrationDate == default)
            return BadRequest("Registration date is required");
        var result = await readerService.CreateAsync(input);
        logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), result.Id);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Обновить данные читателя.
    /// PUT: /api/readers/{id}
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для обновления</param>
    /// <param name="input">DTO с новыми данными читателя</param>
    /// <returns>Обновленный читатель или 404 Not Found если читатель не найден</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ReaderCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        if (input == null)
            return BadRequest("Reader data is required");
        if (string.IsNullOrWhiteSpace(input.FullName))
            return BadRequest("Reader full name is required");
        if (input.RegistrationDate == default)
            return BadRequest("Registration date is required");
        var result = await readerService.UpdateAsync(id, input);
        logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Удалить читателя по идентификатору.
    /// DELETE: /api/readers/{id}
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    /// <returns>204 No Content при успешном удалении</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(DeleteAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        await readerService.DeleteAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));
        return NoContent();
    }
}
