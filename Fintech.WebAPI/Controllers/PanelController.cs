using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class PanelController(IPanelService _panelService, IUserService _userService) : ControllerBase
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
            var pymesList = await _panelService.GetAllCreditApplicationAsync(page, pageSize, status);
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