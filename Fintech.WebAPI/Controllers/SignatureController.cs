using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Signature;
using Fintech.Application.Interfaces.CreditApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SignatureController(ISignatureService _creditApplicationService) : ControllerBase
{
    /// <summary>
    /// Firma la aceptación del crédito.
    /// </summary>
    /// <param name="request">Información a firmar</param>
    /// <returns>Resultado de la firma</returns>
    [HttpPost("Sign")]
    [ProducesResponseType(typeof(AuditAcceptanceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignAgreement([FromBody] SignAgreementRequest request)
    {
        string host = Request.Headers.Host.ToString();
        string userAgent = Request.Headers.UserAgent.ToString();

        var auditAcceptance = new AuditAcceptanceDto
        {
            UserId = request.UserId,
            CreditId = request.CreditId,
            DocumentText = request.DocumentText,
            Host = host,
            UserAgent = userAgent
        };
        var response = await _creditApplicationService.SignAgreementAsync(auditAcceptance);
        return Ok(response);
    }

    /// <summary>
    /// Validación de la firma del crédito.
    /// </summary>
    /// <param name="request">Información a validar</param>
    /// <returns>Resultado de la validación de firma</returns>
    [HttpPost("Verify")]
    [ProducesResponseType(typeof(VerifySignatureDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifySignature([FromBody] VerifySignatureRequest request)
    {
        var auditAcceptance = new SignatureDto
        {
            UserId = request.UserId,
            CreditId = request.CreditId,
            DocumentText = request.DocumentText,
        };
        var response = await _creditApplicationService.VerifySignatureAsync(auditAcceptance);
        return Ok(response);
    }
}
