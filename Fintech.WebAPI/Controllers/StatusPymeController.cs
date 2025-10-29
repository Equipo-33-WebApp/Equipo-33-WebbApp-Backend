using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/status-pyme")]
public class StatusPymeController : ControllerBase
{
    private readonly IStatusPymeService _statusPymeService;

    public StatusPymeController(IStatusPymeService statusPymeService)
    {
        _statusPymeService = statusPymeService;
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(StatusPymeSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusPymeSummaryDto>> GetSummary()
    {
        try
        {
            var summary = await _statusPymeService.GetStatusSummaryAsync();
            return Ok(summary);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}