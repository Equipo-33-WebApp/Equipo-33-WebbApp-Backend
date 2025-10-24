using Fintech.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CreditFormController : ControllerBase
    {

        /// <summary>
        /// Obtiene un CreditForm por su ID.
        /// </summary>
        /// <param name="id">ID del CreditForm a obtener.</param>
        /// <returns>Información de CreditForm junto a sus documentos.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return null!;
        }

        /// <summary>
        /// Crea un CreditForm.
        /// </summary>
        /// <returns>Statuts created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCreditForm(CreateCreditFormDto createCreditFormDto)
        {
            return null;
        }
    }
}