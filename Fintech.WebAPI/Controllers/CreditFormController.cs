using Fintech.Application.Interfaces.CreditApplication;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditFormController : ControllerBase
    {
        private readonly ICreditFormService _creditFormService;
        public CreditFormController(ICreditFormService creditFormService)
        {
            _creditFormService = creditFormService;
        }

        /// <summary>
        /// Obtiene una CreditForm por su ID.
        /// </summary>
        /// <param name="id">ID de la Pyme a obtener.</param>
        /// <returns>Información de la Pyme.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var pyme = await _creditFormService.GetByIdAsync(id);
                if (pyme == null)
                    return NotFound("No se encontro el creditForm");

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
    }
}