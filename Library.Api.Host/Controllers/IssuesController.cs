using Library.Application.Contracts.Issues;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssuesController : ControllerBase
{
    private readonly IIssueService _issueService;

    public IssuesController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    // GET: api/issues
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<IssueDto>>> GetListAsync()
    {
        var result = await _issueService.GetListAsync();
        return Ok(result);
    }

    // GET: api/issues/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IssueDto>> GetAsync(int id)
    {
        var result = await _issueService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST: api/issues
    [HttpPost]
    public async Task<ActionResult<IssueDto>> CreateAsync([FromBody] IssueCreateUpdateDto input)
    {
        var result = await _issueService.CreateAsync(input);
        return CreatedAtAction(
            nameof(GetAsync),
            new { id = result.Id },
            result);
    }

    // PUT: api/issues/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<IssueDto>> UpdateAsync(int id, [FromBody] IssueCreateUpdateDto input)
    {
        var result = await _issueService.UpdateAsync(id, input);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // DELETE: api/issues/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _issueService.DeleteAsync(id);
        return NoContent();
    }
}
