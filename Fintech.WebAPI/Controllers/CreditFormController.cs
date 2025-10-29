using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintech.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CreditFormController : ControllerBase
    {
        private readonly ICreditFormService _creditFormService;
        private readonly IPymeService _pymeService;
        public CreditFormController(ICreditFormService creditFormService, IPymeService pymeService)
        {
            _creditFormService = creditFormService;
            _pymeService = pymeService;
        }

        /// <summary>
        /// Obtiene un CreditForm por su ID.
        /// </summary>
        /// <param name="id">ID del CreditForm a obtener.</param>
        /// <returns>Información de CreditForm junto a sus documentos.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var pyme = await _creditFormService.GetByIdAsync(id);
                if (pyme == null)
                    return NotFound("No se encontre el CreditForm ingresada");

                return Ok(pyme);
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
        /// Crea una nueva solicitud de Credito.
        /// </summary>
        /// <param name="dto">El DTO del form a crear  a crear.</param>
        /// <returns>La Pyme creada.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCreditForm([FromBody] CreateCreditFormDto dto)
        {
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(subClaim))
                return Unauthorized("Revisa si el token es valido");

            if (!Guid.TryParse(subClaim, out var authId))
                return Unauthorized("El guid no es valido.");
            try
            {
                var pyme = await _pymeService.GetByIdAsync(dto.PymeId);
                if (pyme == null)
                    return BadRequest("La pyme no existe.");

                var createdPyme = await _creditFormService.CreateAsync(dto, authId);
                return CreatedAtAction(nameof(GetById), new { id = createdPyme.Id }, createdPyme);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error interno al crear el CreditForm. Puede ser un problema de RLS, llave foranea, o configuracion de Supabase.",
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        /// <summary>
        /// Actualiza la solicitud de Credito.
        /// </summary>
        /// <param name="dto">El DTO del form a actualizar .</param>
        /// <returns>La Solicitud de credito fue creada.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCreditForm([FromBody] UpdateCreditFormDto dto)
        {
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(subClaim))
                return Unauthorized("Revisa si el token es valido");
            if (!Guid.TryParse(subClaim, out var authId))
                return Unauthorized("El guid no es valido.");
            try
            {
                var updatedCreditForm = await _creditFormService.UpdateAsync(dto, authId);
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
        /// Actualiza un CreditForm por su ID.
        /// </summary>
        /// <param name="creditFormId">ID del CreditForm a actualizar.</param>
        /// <returns>Información de CreditForm junto a sus documentos.</returns>
        [HttpPut("{creditFormId}")]
        public async Task<IActionResult> UpdateCreditFormById(Guid creditFormId, [FromBody] UpdateCreditFormDto dto)
        {
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(subClaim))
                return Unauthorized("Revisa si el token es valido");
            if (!Guid.TryParse(subClaim, out var authId))
                return Unauthorized("El guid no es valido.");
            try
            {
                var existingCreditForm = await _creditFormService.GetByIdAsync(creditFormId);
                if (existingCreditForm == null || existingCreditForm.UserId != authId)
                    return NotFound("No se encontro la solicitud de credito para actualizar o no pertenece al usuario.");
                var updatedCreditForm = await _creditFormService.UpdateAsync(dto, authId);
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
    }
}