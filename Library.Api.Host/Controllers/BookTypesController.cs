using Library.Application.Contracts.BookTypes;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления типами книг.
/// Все ошибки пробрасываются наверх и обрабатываются глобальным ExceptionHandlingMiddleware:
/// - ArgumentException / ArgumentNullException → 400
/// - KeyNotFoundException → 404
/// - InvalidOperationException → 409
/// - остальные исключения → 500
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookTypesController(IBookTypeService bookTypeService, ILogger<BookTypesController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех типов книг.
    /// HTTP GET: /api/booktypes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookTypeDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetListAsync));

        var result = await bookTypeService.GetListAsync();

        if (result.Count == 0)
        {
            logger.LogInformation("{Method} executed: no book types found (204)", nameof(GetListAsync));
            return NoContent(); // 204
        }

        logger.LogInformation("{Method} executed successfully with {Count} items (200)",
            nameof(GetListAsync), result.Count);

        return Ok(result); // 200
    }

    /// <summary>
    /// Получить тип книги по уникальному идентификатору.
    /// HTTP GET: /api/booktypes/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookTypeDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(GetAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        var result = await bookTypeService.GetAsync(id) ?? throw new KeyNotFoundException($"Тип книги с ID {id} не найден");
        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(GetAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Создать новый тип книги.
    /// ID генерируется автоматически на сервере.
    /// HTTP POST: /api/booktypes
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BookTypeDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] BookTypeCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called", nameof(CreateAsync));

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Название типа книги (Name) не может быть пустым");

        var result = await bookTypeService.CreateAsync(input);

        logger.LogInformation("{Method} executed successfully with id={Id} (201)",
            nameof(CreateAsync), result.Id);

        return Created($"/api/booktypes/{result.Id}", result); // 201
    }

    /// <summary>
    /// Обновить данные существующего типа книги.
    /// HTTP PUT: /api/booktypes/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookTypeDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookTypeCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Название типа книги (Name) не может быть пустым");

        var result = await bookTypeService.UpdateAsync(id, input) ?? throw new KeyNotFoundException($"Тип книги с ID {id} не найден для обновления");
        logger.LogInformation("{Method} executed successfully with id={Id} (200)",
            nameof(UpdateAsync), id);

        return Ok(result); // 200
    }

    /// <summary>
    /// Удалить тип книги из системы по идентификатору.
    /// HTTP DELETE: /api/booktypes/{id}
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
            await bookTypeService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Тип книги с ID {id} не найден для удаления");
        }

        logger.LogInformation("{Method} executed successfully with id={Id} (204)",
            nameof(DeleteAsync), id);

        return NoContent(); // 204
    }
}
