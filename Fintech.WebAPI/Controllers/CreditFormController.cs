using Fintech.Application.DTOs;
using Fintech.Application.Interfaces;
using Fintech.Application.Interfaces.CreditApplication;
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
        /// Obtiene una solicitud de Credito por su ID.
        /// </summary>
        /// <param name="id">ID de la solicitud de Credito a obtener.</param>
        /// <returns>Información de la solicitud de Credito junto a sus documentos.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var existingCreditForm = await _creditFormService.GetByIdAsync(id);
                if (existingCreditForm == null)
                    return NotFound("No se encontre el CreditForm ingresada");

                return Ok(existingCreditForm);
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
        /// Obtiene una solicitud de Credito en estado Draft del usuario actual.
        /// </summary>
        /// <returns>Información de la solicitud de Credito junto a sus documentos.</returns>
        [HttpGet("auth")]
        public async Task<IActionResult> GetDraftByAuthId()
        {
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(subClaim))
                return Unauthorized("Revisa si el token es valido");
            if (!Guid.TryParse(subClaim, out var authId))
                return Unauthorized("El guid no es valido.");

            try
            {
                var existingCreditForm = await _creditFormService.GetDraftByAuthIdAsync(authId);
                if (existingCreditForm == null)
                    return NotFound("No se encontro un draft de CreditForm");

                return Ok(existingCreditForm);
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

                var existingCreditForm = await _creditFormService.GetDraftByAuthIdAsync(authId);
                if (existingCreditForm != null)
                    return Conflict(new
                    {
                        message = $"Ya existe una solicitud de crédito en estado draft: {existingCreditForm.Id}",
                        existing = existingCreditForm
                    });

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
        /// Actualiza la solicitud de Credito en estado draft del usuario actual.
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
                var existingCreditForm = await _creditFormService.GetDraftByAuthIdAsync(authId);
                if (existingCreditForm == null)
                    return NotFound("No se encontro un draft de CreditForm");

                var updatedCreditForm = await _creditFormService.UpdateAsync(dto, existingCreditForm);
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
        /// Actualiza una solicitud de Credito por su ID.
        /// </summary>
        /// <param name="creditFormId">ID de la solicitud de Credito a actualizar.</param>
        /// <returns>Información de solicitud de Credito junto a sus documentos.</returns>
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
                if (existingCreditForm == null || existingCreditForm.Status == "Pending")
                    return NotFound("La solicitud de crédito ya fue enviada a revisar, no puede ser actualizada.");
                var updatedCreditForm = await _creditFormService.UpdateAsync(dto, existingCreditForm);
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