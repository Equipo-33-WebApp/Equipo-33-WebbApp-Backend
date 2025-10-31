using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Aml;
using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.Aml;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Domain.Entities.Panel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintech.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class OperatorPanelController(IPanelService _panelService, IUserService _userService, IAmlService _amlService) : ControllerBase
{
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
        if (page < 1)
            return BadRequest("El número de página debe ser mayor o igual a 1.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("El tamaño de página debe estar entre 1 y 100.");

        if (await _userService.IsRoleAsync(Roles.OPERATOR))
            return Unauthorized("Usuario no tiene el rol de operador.");

        try
        {
            var filter = new CreditFilter(Page: page, PageSize: pageSize, Status: status);
            var pymesList = await _panelService.GetAllCreditApplicationAsync(filter);
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

    /// <summary>
    /// Actualiza el estado de una solicitud de Credito por su ID.
    /// </summary>
    /// <param name="creditFormId">ID de la solicitud de Credito a actualizar.</param>
    /// <param name="dto">El DTO del form a actualizar .</param>
    /// <returns>Información de la solicitud de Credito.</returns>
    [HttpPut("{creditFormId}")]
    public async Task<IActionResult> UpdateCreditFormById(Guid creditFormId, [FromBody] UpdateStatusCreditFormDto dto)
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(subClaim))
            return Unauthorized("Revisa si el token es valido");
        if (!Guid.TryParse(subClaim, out var authId))
            return Unauthorized("El guid no es valido.");

        if (await _userService.IsRoleAsync(Roles.OPERATOR))
            return Unauthorized("Usuario no tiene el rol de operador.");

        try
        {
            var updatedCreditForm = await _panelService.UpdateStatusAsync(dto, creditFormId);
            if (updatedCreditForm == null)
                return NotFound("No se encontro la solicitud de credito para actualizar.");
            return Ok(updatedCreditForm);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Error interno al actualizar la solicitud de credito. Puede ser un problema de RLS, llave foranea, o configuracion de Supabase.",
                message = ex.Message,
                stackTrace = ex.StackTrace
            }
            );

        }
    }

    /// <summary>
    /// Obtiene todas las verificaciones AML por pyme
    /// </summary>
    /// <param name="id">ID de la Pyme.</param>
    /// <returns>Listado de validaciones AML.</returns>
    [HttpGet("aml-by-pyme{id}")]
    public async Task<ActionResult<AmlResultDto>> GetByPymeIdAsync(Guid id)
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(subClaim))
            return Unauthorized("Revisa si el token es valido");
        if (!Guid.TryParse(subClaim, out var authId))
            return Unauthorized("El guid no es valido.");

        if (await _userService.IsRoleAsync(Roles.OPERATOR))
            return Unauthorized("Usuario no tiene el rol de operador.");

        try
        {
            var result = await _amlService.GetAllByPymeIdsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Error interno al obtener AML-checks. Puede ser un problema de RLS, llave foranea, o configuracion de Supabase.",
                message = ex.Message,
                stackTrace = ex.StackTrace
            }
            );
        }
    }
}