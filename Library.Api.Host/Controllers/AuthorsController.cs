using Library.Application.Contracts.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    // GET: api/authors
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuthorDto>>> GetListAsync()
    {
        var result = await _authorService.GetListAsync();
        return Ok(result);
    }

    // GET: api/authors/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetAsync(int id)
    {
        var result = await _authorService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST: api/authors
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAsync([FromBody] AuthorCreateUpdateDto input)
    {
        var result = await _authorService.CreateAsync(input);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    // PUT: api/authors/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuthorDto>> UpdateAsync(int id, [FromBody] AuthorCreateUpdateDto input)
    {
        var result = await _authorService.UpdateAsync(id, input);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // DELETE: api/authors/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _authorService.DeleteAsync(id);
        return NoContent();
    }
}
