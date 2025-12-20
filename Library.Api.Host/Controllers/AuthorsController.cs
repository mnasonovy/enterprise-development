using Library.Application.Contracts.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(
    IAuthorService authorService,
    ILogger<AuthorsController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<AuthorDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} called", nameof(GetListAsync));
        var result = await authorService.GetListAsync();

        if (result.Count == 0)
        {
            logger.LogInformation("{Method} executed: no authors found (204)", nameof(GetListAsync));
            return NoContent(); // 204
        }

        logger.LogInformation("{Method} executed successfully with {Count} items (200)", nameof(GetListAsync), result.Count);
        return Ok(result); // 200
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(GetAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        var result = await authorService.GetAsync(id)
            ?? throw new KeyNotFoundException($"Автор с ID {id} не найден");

        logger.LogInformation("{Method} executed successfully with id={Id} (200)", nameof(GetAsync), id);
        return Ok(result); // 200
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] AuthorCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called", nameof(CreateAsync));
        if (!ModelState.IsValid)
        {

        }
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (input.Id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {input.Id}");

        if (string.IsNullOrWhiteSpace(input.LastName))
            throw new ArgumentException("Фамилия не может быть пустой");

        var result = await authorService.CreateAsync(input);

        logger.LogInformation("{Method} executed successfully with id={Id} (201)", nameof(CreateAsync), result.Id);
        return Created($"/api/authors/{result.Id}", result); // 201
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(AuthorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorCreateUpdateDto input)
    {
        logger.LogInformation("{Method} called with id={Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            throw new ArgumentException($"ID должен быть больше 0, получено: {id}");

        if (input == null)
            throw new ArgumentNullException(nameof(input), "Тело запроса не может быть пусто");

        if (string.IsNullOrWhiteSpace(input.LastName))
            throw new ArgumentException("Фамилия не может быть пустой");

        var result = await authorService.UpdateAsync(id, input) ?? throw new KeyNotFoundException($"Автор с ID {id} не найден для обновления");
        logger.LogInformation("{Method} executed successfully with id={Id} (200)", nameof(UpdateAsync), id);
        return Ok(result); // 200
    }

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
            await authorService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Автор с ID {id} не найден для удаления");
        }

        logger.LogInformation("{Method} executed successfully with id={Id} (204)", nameof(DeleteAsync), id);
        return NoContent(); // 204
    }
}
