using Library.Application.Contracts.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления издателями.
/// Все ошибки отдаются через глобальный ExceptionHandlingMiddleware:
/// - ArgumentException / ArgumentNullException → 400
/// - KeyNotFoundException → 404
/// - InvalidOperationException → 409
/// - остальные исключения → 500
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
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<PublisherDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetListAsync));

        var result = await publisherService.GetListAsync();

        if (result.Count == 0)
        {
            logger.LogInformation("{Method} executed: no publishers found (204)", nameof(GetListAsync));
            return NoContent(); // 204
        }

        logger.LogInformation("{Method} executed successfully with {Count} items (200)",
            nameof(GetListAsync), result.Count);

        return Ok(result); // 200
    }

    /// <summary>
    /// Получить издателя по уникальному идентификатору.
    /// HTTP GET: /api/publishers/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(PublisherDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(GetAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        var result = await publisherService.GetAsync(id);

        if (result == null)
            throw new KeyNotFoundException($"Издатель с ID {id} не найден");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(GetAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Создать нового издателя.
    /// HTTP POST: /api/publishers
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PublisherDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] PublisherCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called", nameof(CreateAsync));

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Publisher data is required");

        if (input.Id <= 0)
            throw new ArgumentException($"Publisher ID must be greater than 0, получено: {input.Id}");

        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Publisher name is required");

        var result = await publisherService.CreateAsync(input);

        logger.LogInformation("{Method} executed successfully with id={Id} (201)",
            nameof(CreateAsync), result.Id);

        return Created($"/api/publishers/{result.Id}", result); // 201
    }

    /// <summary>
    /// Обновить данные существующего издателя.
    /// HTTP PUT: /api/publishers/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(PublisherDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] PublisherCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Publisher data is required");

        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Publisher name is required");

        var result = await publisherService.UpdateAsync(id, input);

        if (result == null)
            throw new KeyNotFoundException($"Издатель с ID {id} не найден для обновления");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(UpdateAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Удалить издателя по идентификатору.
    /// HTTP DELETE: /api/publishers/{id}
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
            await publisherService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Издатель с ID {id} не найден для удаления");
        }

        logger.LogInformation("{Method} executed successfully with id={Id} (204)",
            nameof(DeleteAsync), id);

        return NoContent(); // 204
    }
}
