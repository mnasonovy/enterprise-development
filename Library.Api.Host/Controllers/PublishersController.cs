using Library.Application.Contracts.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    // GET: api/publishers
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PublisherDto>>> GetListAsync()
    {
        var result = await _publisherService.GetListAsync();
        return Ok(result);
    }

    // GET: api/publishers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PublisherDto>> GetAsync(int id)
    {
        var result = await _publisherService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST: api/publishers
    [HttpPost]
    public async Task<ActionResult<PublisherDto>> CreateAsync([FromBody] PublisherCreateUpdateDto input)
    {
        var result = await _publisherService.CreateAsync(input);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    // PUT: api/publishers/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PublisherDto>> UpdateAsync(int id, [FromBody] PublisherCreateUpdateDto input)
    {
        var result = await _publisherService.UpdateAsync(id, input);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // DELETE: api/publishers/{id}
    [HttpDelete("{id:int}")]
    public async Task DeleteAsync(int id)
    {
        await _publisherService.DeleteAsync(id);
        NoContent();
    }
}