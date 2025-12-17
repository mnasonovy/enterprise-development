using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления читателями.
/// Реализует все CRUD-операции: получение списка, получение по ID, создание, обновление и удаление.
/// Все методы асинхронные с полной валидацией входных данных и логированием.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReadersController(
    IReaderService readerService,
    ILogger<ReadersController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех читателей.
    /// HTTP GET: /api/readers
    /// Возвращает 200 OK с полным списком или 204 No Content если список пуст.
    /// </summary>
    /// <returns>IReadOnlyList&lt;ReaderDto&gt; - список всех читателей</returns>
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
    /// Получить читателя по уникальному идентификатору.
    /// HTTP GET: /api/readers/{id}
    /// Валидирует ID (должен быть больше 0).
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>ReaderDto если найден, иначе NotFound</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ReaderDto))]
    [ProducesResponseType(204)]
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
    /// Создать нового читателя в системе.
    /// HTTP POST: /api/readers
    /// Требует валидные данные: FullName, RegistrationDate, ID должен быть установлен вручную и > 0.
    /// Возвращает 201 Created с заполненными данными и ID.
    /// </summary>
    /// <param name="input">DTO с данными нового читателя (FullName, Id, RegistrationDate обязательны)</param>
    /// <returns>Created с ReaderDto и его ID</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] ReaderCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Reader data is required");

        if (input.Id <= 0)
            return BadRequest("Reader ID must be set manually and be greater than 0");

        if (string.IsNullOrWhiteSpace(input.FullName))
            return BadRequest("Reader full name is required");

        if (input.RegistrationDate == default)
            return BadRequest("Registration date is required");

        try
        {
            var result = await readerService.CreateAsync(input);

            logger.LogInformation("{Method} method executed successfully with id = {Id}",
                nameof(CreateAsync), result.Id);

            return Created($"/api/readers/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error creating Reader: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные существующего читателя.
    /// HTTP PUT: /api/readers/{id}
    /// Валидирует ID и входные данные перед обновлением.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для обновления</param>
    /// <param name="input">DTO с новыми данными читателя (FullName, RegistrationDate обязательны)</param>
    /// <returns>Ok с обновленными данными или NotFound если читатель не существует</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ReaderDto))]
    [ProducesResponseType(204)]
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

        try
        {
            var result = await readerService.UpdateAsync(id, input);

            logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

            return result != null ? Ok(result) : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error updating Reader: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удалить читателя из системы по идентификатору.
    /// HTTP DELETE: /api/readers/{id}
    /// Валидирует ID перед удалением. Возвращает 204 No Content при успехе.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    /// <returns>NoContent (204) при успешном удалении</returns>
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
            await readerService.DeleteAsync(id);

            logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error deleting Reader: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
