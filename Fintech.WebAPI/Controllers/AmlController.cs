using Fintech.Application.DTOs.Aml;
using Fintech.Application.Interfaces.Aml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AmlController : ControllerBase
{
    private readonly IAmlService _amlService;

    public AmlController(IAmlService amlService)
    {
        _amlService = amlService;
    }

    /// <summary>
    /// Realiza una verificación AML para un usuario.
    /// </summary>
    /// <param name="amlRequest">Datos de la solicitud AML.</param>
    /// <returns>Resultado de la verificación AML.</returns>
    [HttpPost("check")]
    public async Task<ActionResult<AmlResultDto>> CheckAmlAsync([FromBody] AmlRequestDto amlRequest)
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(subClaim))
        {
            return Unauthorized("El token de autenticación no contiene el ID de usuario.");
        }

        if (!Guid.TryParse(subClaim, out var authId))
        {
            return BadRequest("El ID de usuario en el token no es un GUID válido.");
        }
        var result = await _amlService.CheckAsync(amlRequest, authId);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene todas las verificaciones AML para el usuario autenticado.
    /// </summary>
    /// <returns>Una lista de resultados de verificaciones AML.</returns>
    [HttpGet("my-checks")]
    public async Task<ActionResult<IEnumerable<AmlResultDto>>> GetMyChecksAsync()
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(subClaim))
        {
            return Unauthorized("El token de autenticación no contiene el ID de usuario.");
        }

        if (!Guid.TryParse(subClaim, out var authId))
        {
            return BadRequest("El ID de usuario en el token no es un GUID válido.");
        }

        var results = await _amlService.GetChecksByAuthIdAsync(authId);
        return Ok(results);
    }

    /// <summary>
    /// Obtiene una verificación AML por su ID.
    /// </summary>
    /// <param name="id">ID de la verificación AML.</param>
    /// <returns>Resultado de la verificación AML.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AmlResultDto>> GetByIdAsync(Guid id)
    {
        var result = await _amlService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
}