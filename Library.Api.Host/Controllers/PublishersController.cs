using Library.Application.Contracts.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над издателями
/// 🔧 ИСПРАВЛЕНО: DELETE return statement, логирование, валидация
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PublishersController(
    IPublisherService publisherService,
    ILogger<PublishersController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех издателей
    /// GET: /api/publishers
    /// </summary>
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
    /// Получить издателя по идентификатору
    /// GET: /api/publishers/{id}
    /// </summary>
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
    /// Создать нового издателя
    /// POST: /api/publishers
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PublisherDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] PublisherCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Publisher data is required");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("Publisher name is required");

        var result = await publisherService.CreateAsync(input);

        logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), result.Id);

        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Обновить данные издателя
    /// PUT: /api/publishers/{id}
    /// </summary>
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

        var result = await publisherService.UpdateAsync(id, input);

        logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Удалить издателя по идентификатору
    /// DELETE: /api/publishers/{id}
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

        await publisherService.DeleteAsync(id);

        logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

        return NoContent(); // 🔧 ДОБАВЛЕНО return!
    }
}
