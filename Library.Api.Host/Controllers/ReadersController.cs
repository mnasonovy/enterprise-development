using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления читателями.
/// Все ошибки обрабатываются глобальным ExceptionHandlingMiddleware:
/// - ArgumentException / ArgumentNullException → 400
/// - KeyNotFoundException → 404
/// - InvalidOperationException → 409
/// - остальные исключения → 500
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
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<ReaderDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetListAsync));

        var result = await readerService.GetListAsync();

        if (result.Count == 0)
        {
            logger.LogInformation("{Method} executed: no readers found (204)", nameof(GetListAsync));
            return NoContent(); // 204
        }

        logger.LogInformation("{Method} executed successfully with {Count} items (200)",
            nameof(GetListAsync), result.Count);

        return Ok(result); // 200
    }

    /// <summary>
    /// Получить читателя по ID.
    /// HTTP GET: /api/readers/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(GetAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        var result = await readerService.GetAsync(id) ?? throw new KeyNotFoundException($"Читатель с ID {id} не найден");
        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(GetAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Создать нового читателя.
    /// HTTP POST: /api/readers
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] ReaderCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called", nameof(CreateAsync));

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Reader data is required");

        if (input.Id <= 0)
            throw new ArgumentException($"Reader ID must be greater than 0, получено: {input.Id}");

        if (string.IsNullOrWhiteSpace(input.FullName))
            throw new ArgumentException("Reader full name is required");

        if (input.RegistrationDate == default)
            throw new ArgumentException("Registration date is required");

        var result = await readerService.CreateAsync(input);

        logger.LogInformation("{Method} executed successfully with id={Id} (201)",
            nameof(CreateAsync), result.Id);

        return Created($"/api/readers/{result.Id}", result); // 201
    }

    /// <summary>
    /// Обновить данные существующего читателя.
    /// HTTP PUT: /api/readers/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ReaderDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ReaderCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Reader data is required");

        if (string.IsNullOrWhiteSpace(input.FullName))
            throw new ArgumentException("Reader full name is required");

        if (input.RegistrationDate == default)
            throw new ArgumentException("Registration date is required");

        var result = await readerService.UpdateAsync(id, input) ?? throw new KeyNotFoundException($"Читатель с ID {id} не найден для обновления");
        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(UpdateAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Удалить читателя по ID.
    /// HTTP DELETE: /api/readers/{id}
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
            await readerService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Читатель с ID {id} не найден для удаления");
        }

        logger.LogInformation("{Method} executed successfully with id={Id} (204)",
            nameof(DeleteAsync), id);

        return NoContent(); // 204
    }
}
