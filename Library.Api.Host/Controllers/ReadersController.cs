using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadersController : ControllerBase
{
    private readonly IReaderService _readerService;

    public ReadersController(IReaderService readerService)
    {
        _readerService = readerService;
    }

    // GET: api/readers
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReaderDto>>> GetListAsync()
    {
        var result = await _readerService.GetListAsync();
        return Ok(result);
    }

    // GET: api/readers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReaderDto>> GetAsync(int id)
    {
        var result = await _readerService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST: api/readers
    [HttpPost]
    public async Task<ActionResult<ReaderDto>> CreateAsync([FromBody] ReaderCreateUpdateDto input)
    {
        var result = await _readerService.CreateAsync(input);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    // PUT: api/readers/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReaderDto>> UpdateAsync(int id, [FromBody] ReaderCreateUpdateDto input)
    {
        var result = await _readerService.UpdateAsync(id, input);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // DELETE: api/readers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _readerService.DeleteAsync(id);
        return NoContent();
    }
}
