using Library.Application.Contracts.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления издателями.
/// Реализует все CRUD-операции: получение списка, получение по ID, создание, обновление и удаление.
/// Все методы асинхронные с полной валидацией входных данных и логированием.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PublishersController(
    IPublisherService publisherService,
    ILogger<PublishersController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех издателей.
    /// HTTP GET: /api/publishers
    /// Возвращает 200 OK с полным списком или 204 No Content если список пуст.
    /// </summary>
    /// <returns>IReadOnlyList&lt;PublisherDto&gt; - список всех издателей</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<PublisherDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));
        var result = await publisherService.GetListAsync();
        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);
        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить издателя по уникальному идентификатору.
    /// HTTP GET: /api/publishers/{id}
    /// Валидирует ID (должен быть больше 0).
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>PublisherDto если найден, иначе NotFound</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(PublisherDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        var result = await publisherService.GetAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать нового издателя в системе.
    /// HTTP POST: /api/publishers
    /// Требует валидное имя издателя (не пусто и не только пробелы).
    /// ID должен быть установлен вручную и быть больше 0.
    /// Возвращает 201 Created с заполненными данными и ID.
    /// </summary>
    /// <param name="input">DTO с данными нового издателя (Name и Id обязательны)</param>
    /// <returns>Created с PublisherDto и его ID</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PublisherDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] PublisherCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Publisher data is required");

        if (input.Id <= 0)
            return BadRequest("Publisher ID must be set manually and be greater than 0");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("Publisher name is required");

        try
        {
            var result = await publisherService.CreateAsync(input);

            logger.LogInformation("{Method} method executed successfully with id = {Id}",
                nameof(CreateAsync), result.Id);

            return Created($"/api/publishers/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error creating Publisher: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные существующего издателя.
    /// HTTP PUT: /api/publishers/{id}
    /// Валидирует ID и входные данные перед обновлением.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для обновления</param>
    /// <param name="input">DTO с новыми данными издателя (Name обязателен)</param>
    /// <returns>Ok с обновленными данными или NotFound если издатель не существует</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(PublisherDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] PublisherCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("Publisher data is required");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("Publisher name is required");

        try
        {
            var result = await publisherService.UpdateAsync(id, input);

            logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

            return result != null ? Ok(result) : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error updating Publisher: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удалить издателя из системы по идентификатору.
    /// HTTP DELETE: /api/publishers/{id}
    /// Валидирует ID перед удалением. Возвращает 204 No Content при успехе.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для удаления</param>
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
            await publisherService.DeleteAsync(id);

            logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error deleting Publisher: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
