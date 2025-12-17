using Library.Application.Contracts.Issues;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для управления выданными книгами (Issue).
/// Предоставляет REST API endpoints для выполнения CRUD операций над выданными книгами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IssuesController(
    IIssueService issueService,
    ILogger<IssuesController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех выданных книг.
    /// GET: /api/issues
    /// </summary>
    /// <returns>Список всех выданных книг или 204 No Content если список пуст</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<IssueDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));
        var result = await issueService.GetListAsync();
        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);
        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить запись о выданной книге по идентификатору.
    /// GET: /api/issues/{id}
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи</param>
    /// <returns>Выданная книга если найдена, иначе 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        var result = await issueService.GetAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать новую запись о выданной книге.
    /// POST: /api/issues
    /// </summary>
    /// <param name="input">DTO с данными выдачи (BookId, ReaderId, IssueDate, DaysCount обязательны)</param>
    /// <returns>Созданная выданная книга с кодом 201 Created</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] IssueCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));
        if (input == null)
            return BadRequest("Issue data is required");
        if (input.ReaderId <= 0)
            return BadRequest("Valid reader id is required");
        if (input.BookId <= 0)
            return BadRequest("Valid book id is required");
        if (input.DaysCount <= 0)
            return BadRequest("Days count must be greater than 0");
        var result = await issueService.CreateAsync(input);
        logger.LogInformation("{Method} method executed successfully with id = {Id}",
            nameof(CreateAsync), result.Id);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Обновить запись о выданной книге.
    /// PUT: /api/issues/{id}
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для обновления</param>
    /// <param name="input">DTO с новыми данными выдачи</param>
    /// <returns>Обновленная выданная книга или 404 Not Found если запись не найдена</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] IssueCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        if (input == null)
            return BadRequest("Issue data is required");
        if (input.ReaderId <= 0)
            return BadRequest("Valid reader id is required");
        if (input.BookId <= 0)
            return BadRequest("Valid book id is required");
        if (input.DaysCount <= 0)
            return BadRequest("Days count must be greater than 0");
        var result = await issueService.UpdateAsync(id, input);
        logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Удалить запись о выданной книге по идентификатору.
    /// DELETE: /api/issues/{id}
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для удаления</param>
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
        await issueService.DeleteAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));
        return NoContent();
    }
}
