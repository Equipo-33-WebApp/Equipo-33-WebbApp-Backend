using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PymesController(IPymeService _pymeService, IPanelService _panelService, IUserService _userService) : ControllerBase
{
    /// <summary>
    /// Obtiene una Pyme por su ID.
    /// </summary>
    /// <param name="id">ID de la Pyme a obtener.</param>
    /// <returns>Información de la Pyme.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var pyme = await _pymeService.GetByIdAsync(id);
            if (pyme == null)
                return NotFound("No se encontre la pyme ingresada");

            return Ok(pyme);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Error interno al consultar la Pyme. Revisa la conexion con supabase y politica RLS",
                message = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// Obtiene una Pyme por su ID de autenticación.
    /// </summary>
    /// <param name="authId">ID de autenticación de la Pyme a obtener.</param>
    /// <returns>Información de la Pyme.</returns>
    [HttpGet("auth/{authId}")]
    public async Task<ActionResult<PymeRequestDto>> GetByAuthId(Guid authId)
    {
        var response = await _pymeService.GetByAuthIdAsync(authId);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }

    /// <summary>
    /// Crea una nueva Pyme.
    /// </summary>
    /// <param name="dto">El DTO de la Pyme a crear.</param>
    /// <returns>La Pyme creada.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PymeRequestDto dto)
    {
        if (dto == null)
            return BadRequest("El DTO recibido es null");

        if (string.IsNullOrWhiteSpace(dto.CompanyName))
            return BadRequest("El campo 'CompanyName' es obligatorio");

        if (string.IsNullOrWhiteSpace(dto.Address))
            return BadRequest("El campo 'Address' es obligatorio");

        if (string.IsNullOrWhiteSpace(dto.Sector))
            return BadRequest("El campo 'Sector' es obligatorio");

        if (dto.Employees < 0)
            return BadRequest("El campo 'Employees' no puede ser negativo");

        if (string.IsNullOrWhiteSpace(dto.Phone))
            return BadRequest("El campo 'Phone' es obligatorio");

        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(subClaim))
            return Unauthorized("Revisa si el token es valido");

        if (!Guid.TryParse(subClaim, out var authId))
            return Unauthorized("El guid no es valido.");

        try
        {
            var existingPyme = await _pymeService.GetByAuthIdAsync(authId);
            if (existingPyme != null)
            {
                return Conflict("El usuario ya tiene una PYME registrada.");
            }

            var createdPyme = await _pymeService.CreateAsync(dto, authId);
            return CreatedAtAction(nameof(GetById), new { id = createdPyme.Id }, createdPyme);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Error interno al crear la Pyme. Puede ser un problema de RLS, llave foranea, o configuracion de Supabase.",
                message = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// Actualiza una Pyme por su ID.
    /// </summary>
    /// <param name="authId">ID de autenticación de la Pyme a actualizar.</param>
    /// <param name="dto">El DTO de la Pyme a actualizar.</param>
    /// <returns>La Pyme actualizada.</returns>
    [HttpPut("{authId}")]
    public async Task<ActionResult<PymeRequestDto>> Update(Guid authId, [FromBody] UpdatePymeDto dto)
    {
        var updatedPyme = await _pymeService.UpdateAsync(authId, dto);
        if (updatedPyme == null)
        {
            return NotFound();
        }

        return Ok(updatedPyme);
    }

    /// <summary>
    /// Elimina una Pyme por su ID.
    /// </summary>
    /// <param name="id">ID de la Pyme a eliminar.</param>
    /// <returns>Respuesta sin contenido.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _pymeService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Obtiene una lista paginada de solicitudes de crédito con filtro de estado opcional.
    /// </summary>
    /// <param name="page">Número de página actual (por defecto 1).</param>
    /// <param name="pageSize">Tamaño de la página (por defecto 10).</param>
    /// <param name="status">Estado opcional para filtrar las solicitudes de crédito.</param>
    /// <returns>Una lista paginada de solicitudes de crédito con detalles.</returns>
    [HttpGet]
    public async Task<IActionResult> GetById(int page = 1, int pageSize = 10, string? status = null)
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(subClaim))
            return Unauthorized("Revisa si el token es valido");
        if (!Guid.TryParse(subClaim, out var authId))
            return Unauthorized("El guid no es valido.");

        if (page < 1)
            return BadRequest("El número de página debe ser mayor o igual a 1.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("El tamaño de página debe estar entre 1 y 100.");

        try
        {
            if (await _userService.IsRoleAsync(Roles.PYME))
                return Unauthorized("Usuario no tiene el rol de Pyme.");

            var pyme = await _pymeService.GetByAuthIdAsync(authId);
            if(pyme == null)
                return NotFound("Pyme no encontrada");

            var pymesList = await _panelService.GetAllCreditApplicationByPymeAsync(pyme.Id, page, pageSize, status);
            var totalCount = pymesList.Count();

            var response = new
            {
                page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                data = pymesList
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Error interno al consultar el CreditForm. Revisa la conexion con supabase y politica RLS",
                message = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }
}
