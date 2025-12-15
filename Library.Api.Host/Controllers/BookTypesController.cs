using Library.Application.Contracts.BookTypes;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookTypesController : ControllerBase
{
    private readonly IBookTypeService _bookTypeService;

    public BookTypesController(IBookTypeService bookTypeService)
    {
        _bookTypeService = bookTypeService;
    }

    // GET: api/booktypes
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookTypeDto>>> GetListAsync()
    {
        var result = await _bookTypeService.GetListAsync();
        return Ok(result);
    }

    // GET: api/booktypes/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookTypeDto>> GetAsync(int id)
    {
        var result = await _bookTypeService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST: api/booktypes
    [HttpPost]
    public async Task<ActionResult<BookTypeDto>> CreateAsync([FromBody] BookTypeCreateUpdateDto input)
    {
        var result = await _bookTypeService.CreateAsync(input);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    // PUT: api/booktypes/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookTypeDto>> UpdateAsync(int id, [FromBody] BookTypeCreateUpdateDto input)
    {
        var result = await _bookTypeService.UpdateAsync(id, input);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // DELETE: api/booktypes/{id}
    [HttpDelete("{id:int}")]
    public async Task DeleteAsync(int id)
    {
        await _bookTypeService.DeleteAsync(id);
        NoContent();
    }
}