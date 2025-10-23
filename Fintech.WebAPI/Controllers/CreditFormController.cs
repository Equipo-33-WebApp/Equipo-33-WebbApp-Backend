using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Factories;
using Fintech.Infrastructure.Persistence.Models;
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
        private readonly SupabaseClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public CreditFormController(
            SupabaseClientFactory clientFactory,
            IMapper mapper)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un CreditForm por su ID.
        /// </summary>
        /// <param name="id">ID del CreditForm a obtener.</param>
        /// <returns>Información de CreditForm junto a sus documentos.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader?.Replace("Bearer ", "").Trim();

            // Crea cliente autenticado
            var supabase = await _clientFactory.CreateClientAsync(token);
            try
            {
                var result = await supabase
                    .From<CreditFormModel>()
                    .Where(x => x.Id == id)
                    .Get();

                var model = result.Models.FirstOrDefault();

                if (model == null)
                {
                    return NotFound(new { message = "CreditForm no encontrada o no tienes permisos para acceder a ella." });
                }
                var creditForm = _mapper.Map<CreditForm>(model);
                return Ok(creditForm);
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
        /// Crea un CreditForm.
        /// </summary>
        /// <returns>Statuts created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCreditForm(CreateCreditFormDto createCreditFormDto)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader?.Replace("Bearer ", "").Trim();
            // Extrae el sub del  token JWT
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;
            // Crea cliente autenticado
            var supabase = await _clientFactory.CreateClientAsync(token);
            try
            {
                var newCreditForm = new CreditFormModel
                {
                    UserId = Guid.Parse(subClaim!),
                    PymeId = createCreditFormDto.PymeId,
                    Amount = createCreditFormDto.Amount,
                    Purpose = createCreditFormDto.Pupose,
                    Status = createCreditFormDto.Status
                };
                var insertResult = await supabase
                    .From<CreditFormModel>()
                    .Insert(newCreditForm);
                var createdModel = insertResult.Model;
                var creditForm = _mapper.Map<CreditForm>(createdModel);
                return CreatedAtAction(nameof(GetById), new { id = creditForm.Id }, creditForm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error interno al crear el CreditForm. Revisa la conexion con supabase y politica RLS",
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                }
                );
            }
        }
    }
}