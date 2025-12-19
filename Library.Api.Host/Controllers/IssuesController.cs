using Library.Application.Contracts.Issues;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для полного управления выданными книгами (Issue).
/// Вся обработка ошибок перенесена в глобальный ExceptionHandlingMiddleware:
/// - ArgumentException / ArgumentNullException → 400
/// - KeyNotFoundException → 404
/// - InvalidOperationException → 409
/// - остальные исключения → 500
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IssuesController(IIssueService issueService, ILogger<IssuesController> logger) : ControllerBase
{
    // GET /api/issues
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<IssueDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetListAsync));

        var result = await issueService.GetListAsync();

        if (result.Count == 0)
        {
            logger.LogInformation("{Method} executed: no issues found (204)", nameof(GetListAsync));
            return NoContent(); // 204
        }

        logger.LogInformation("{Method} executed successfully with {Count} items (200)",
            nameof(GetListAsync), result.Count);

        return Ok(result); // 200
    }

    // GET /api/issues/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(GetAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        var result = await issueService.GetAsync(id);

        if (result == null)
            throw new KeyNotFoundException($"Выдача с ID {id} не найдена");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(GetAsync), id);

        return Ok(result); // 200
    }

    // POST /api/issues
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] IssueCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called", nameof(CreateAsync));

        if (input is null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (input.BookId <= 0)
            throw new ArgumentException($"BookId должен быть больше 0, получено: {input.BookId}");

        if (input.ReaderId <= 0)
            throw new ArgumentException($"ReaderId должен быть больше 0, получено: {input.ReaderId}");

        if (input.IssueDate == default)
            throw new ArgumentException("IssueDate должен быть задан");

        if (input.DaysCount <= 0)
            throw new ArgumentException($"DaysCount должен быть больше 0, получено: {input.DaysCount}");

        var result = await issueService.CreateAsync(input);

        logger.LogInformation("{Method} executed successfully with id={Id} (201)",
            nameof(CreateAsync), result.Id);

        return Created($"/api/issues/{result.Id}", result); // 201
    }

    // PUT /api/issues/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] IssueCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        if (input is null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (input.BookId <= 0)
            throw new ArgumentException($"BookId должен быть больше 0, получено: {input.BookId}");

        if (input.ReaderId <= 0)
            throw new ArgumentException($"ReaderId должен быть больше 0, получено: {input.ReaderId}");

        if (input.IssueDate == default)
            throw new ArgumentException("IssueDate должен быть задан");

        if (input.DaysCount <= 0)
            throw new ArgumentException($"DaysCount должен быть больше 0, получено: {input.DaysCount}");

        var result = await issueService.UpdateAsync(id, input);

        if (result == null)
            throw new KeyNotFoundException($"Выдача с ID {id} не найдена для обновления");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(UpdateAsync), id);

        return Ok(result); // 200
    }

    // DELETE /api/issues/{id}
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
            await issueService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Выдача с ID {id} не найдена для удаления");
        }

        logger.LogInformation("{Method} executed successfully with id={Id} (204)",
            nameof(DeleteAsync), id);

        return NoContent(); // 204
    }
}
