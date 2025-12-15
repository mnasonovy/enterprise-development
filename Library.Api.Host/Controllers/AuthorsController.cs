using Library.Application.Contracts.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над авторами
/// </summary>
/// <param name="authorService">Аппликейшен служба авторов</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить список всех авторов
    /// </summary>
    /// <returns>Список авторов</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IReadOnlyList<AuthorDto>>> GetListAsync()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetListAsync), GetType().Name);
        try
        {
            var result = await authorService.GetListAsync();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetListAsync), GetType().Name);
            return result.Count > 0 ? Ok(result) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetListAsync), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Получить автора по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор автора</param>
    /// <returns>Данные автора</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuthorDto>> GetByIdAsync(int id)
    {
        logger.LogInformation("{method} method of {controller} is called with {id} parameter", nameof(GetByIdAsync), GetType().Name, id);
        try
        {
            var result = await authorService.GetAsync(id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetByIdAsync), GetType().Name);
            return result != null ? Ok(result) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetByIdAsync), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Создать нового автора
    /// </summary>
    /// <param name="input">Данные автора</param>
    /// <returns>Созданный автор</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuthorDto>> CreateAsync([FromBody] AuthorDto input)
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(CreateAsync), GetType().Name);
        try
        {
            var result = await authorService.CreateAsync(input);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(CreateAsync), GetType().Name);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(CreateAsync), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Обновить данные автора
    /// </summary>
    /// <param name="id">Идентификатор автора</param>
    /// <param name="input">Новые данные автора</param>
    /// <returns>Обновленный автор</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuthorDto>> UpdateAsync(int id, [FromBody] AuthorDto input)
    {
        logger.LogInformation("{method} method of {controller} is called with {id} parameter", nameof(UpdateAsync), GetType().Name, id);
        try
        {
            var result = await authorService.UpdateAsync(id, input);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(UpdateAsync), GetType().Name);
            return result != null ? Ok(result) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(UpdateAsync), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Удалить автора по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор автора</param>
    /// <returns>Результат удаления</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{method} method of {controller} is called with {id} parameter", nameof(DeleteAsync), GetType().Name, id);
        try
        {
            await authorService.DeleteAsync(id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(DeleteAsync), GetType().Name);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(DeleteAsync), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}
