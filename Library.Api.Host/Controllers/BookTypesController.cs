using Library.Application.Contracts.BookTypes;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления типами книг.
/// Реализует все CRUD-операции: получение списка, получение по ID, создание, обновление и удаление.
/// Все методы асинхронные с полной валидацией входных данных и логированием.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookTypesController(
    IBookTypeService bookTypeService,
    ILogger<BookTypesController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех типов книг.
    /// HTTP GET: /api/booktypes
    /// Возвращает 200 OK с полным списком или 204 No Content если список пуст.
    /// </summary>
    /// <returns>IReadOnlyList&lt;BookTypeDto&gt; - список всех типов книг</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookTypeDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));
        var result = await bookTypeService.GetListAsync();
        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);
        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить тип книги по уникальному идентификатору.
    /// HTTP GET: /api/booktypes/{id}
    /// Валидирует ID (должен быть больше 0).
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>BookTypeDto если найден, иначе NotFound</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookTypeDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        var result = await bookTypeService.GetAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать новый тип книги в системе.
    /// HTTP POST: /api/booktypes
    /// Требует валидное имя типа (не пусто и не только пробелы).
    /// ID должен быть установлен вручную и быть больше 0.
    /// Возвращает 201 Created с заполненными данными и ID.
    /// </summary>
    /// <param name="input">DTO с данными нового типа (Name и Id обязательны)</param>
    /// <returns>Created с BookTypeDto и его ID</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BookTypeDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] BookTypeCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("BookType data is required");

        if (input.Id <= 0)
            return BadRequest("BookType ID must be set manually and be greater than 0");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("BookType name is required");

        try
        {
            var result = await bookTypeService.CreateAsync(input);

            logger.LogInformation("{Method} method executed successfully with id = {Id}",
                nameof(CreateAsync), result.Id);

            return Created($"/api/booktypes/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error creating BookType: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные существующего типа книги.
    /// HTTP PUT: /api/booktypes/{id}
    /// Валидирует ID и входные данные перед обновлением.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа для обновления</param>
    /// <param name="input">DTO с новыми данными типа (Name обязателен)</param>
    /// <returns>Ok с обновленными данными или NotFound если тип не существует</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookTypeDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookTypeCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("BookType data is required");

        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest("BookType name is required");

        try
        {
            var result = await bookTypeService.UpdateAsync(id, input);

            logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

            return result != null ? Ok(result) : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error updating BookType: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удалить тип книги из системы по идентификатору.
    /// HTTP DELETE: /api/booktypes/{id}
    /// Валидирует ID перед удалением. Возвращает 204 No Content при успехе.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа для удаления</param>
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
            await bookTypeService.DeleteAsync(id);

            logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error deleting BookType: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
