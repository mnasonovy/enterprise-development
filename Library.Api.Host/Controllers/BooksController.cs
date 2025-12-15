using Library.Application.Contracts.Books;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetListAsync()
    {
        var result = await _bookService.GetListAsync();
        return Ok(result);
    }

    // GET: api/books/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetAsync(int id)
    {
        var result = await _bookService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateAsync([FromBody] BookCreateUpdateDto input)
    {
        var result = await _bookService.CreateAsync(input);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    // PUT: api/books/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookDto>> UpdateAsync(int id, [FromBody] BookCreateUpdateDto input)
    {
        var result = await _bookService.UpdateAsync(id, input);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _bookService.DeleteAsync(id);
        return NoContent();
    }
}
