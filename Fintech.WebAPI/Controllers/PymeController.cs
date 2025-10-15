using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintech.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PymesController : ControllerBase
    {
        private readonly IPymeService _pymeService;

        public PymesController(IPymeService pymeService)
        {
            _pymeService = pymeService;
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

            if (!Guid.TryParse(subClaim, out var userId))
                return Unauthorized("El guid no es valido.");

            try
            {
                var createdPyme = await _pymeService.CreateAsync(dto, userId);
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
    }
}
