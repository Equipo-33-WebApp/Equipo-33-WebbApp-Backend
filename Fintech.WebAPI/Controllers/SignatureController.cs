using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Signature;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SignatureController(ISignatureService _creditApplicationService) : ControllerBase
{
    /// <summary>
    /// Firma la aceptación de terminos y condiciones.
    /// </summary>
    /// <param name="request">Información a firmar</param>
    /// <returns>Resultado de la firma</returns>
    [HttpPost("SignText")]
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
        var response = await _creditApplicationService.SignTextSignatureAsync(auditAcceptance);
        return Ok(response);
    }

    /// <summary>
    /// Validación de la firma de terminos y condiciones.
    /// </summary>
    /// <param name="request">Información a validar</param>
    /// <returns>Resultado de la validación de firma</returns>
    [HttpPost("VerifyText")]
    [ProducesResponseType(typeof(VerifySignatureDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifySignature([FromBody] VerifyTextSignatureRequest request)
    {
        var auditAcceptance = new SignatureDto
        {
            UserId = request.UserId,
            CreditId = request.CreditId,
            DocumentText = request.DocumentText,
        };
        var response = await _creditApplicationService.VerifyTextSignatureAsync(auditAcceptance);
        return Ok(response);
    }

    /// <summary>
    /// Firma la aceptación del crédito.
    /// </summary>
    /// <param name="file">Archivo del documento a firmar.</param>
    /// <param name="creditId">ID del crédito.</param>
    /// <param name="userId">ID del usuario.</param>
    /// <returns>Resultado de la firma</returns>
    [HttpPost("SignFile")]
    [ProducesResponseType(typeof(AuditAcceptanceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignDocument(IFormFile file, [FromForm] Guid creditId, [FromForm] Guid userId)
    {
        string host = Request.Headers.Host.ToString();
        string userAgent = Request.Headers.UserAgent.ToString();
        string documentHash = await HashHelper.ComputeSha256Async(file.OpenReadStream());
        var auditAcceptance = new AuditDocumentDto
        {
            UserId = userId,
            CreditId = creditId,
            DocumentHash = documentHash,
            Host = host,
            UserAgent = userAgent
        };
        var response = await _creditApplicationService.SignDocumentAsync(auditAcceptance);
        return Ok(response);
    }

    /// <summary>
    /// Validación de la firma del crédito.
    /// </summary>
    /// <param name="file">Archivo del documento a validar.</param>
    /// <param name="creditId">ID del crédito.</param>
    /// <param name="userId">ID del usuario.</param>
    /// <returns>Resultado de la validación de firma</returns>
    [HttpPost("VerifyFile")]
    [ProducesResponseType(typeof(VerifySignatureDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyFileSignature(IFormFile file, [FromForm] Guid creditId, [FromForm] Guid userId)
    {
        string documentHash = await HashHelper.ComputeSha256Async(file.OpenReadStream());
        var auditAcceptance = new SignatureDto
        {
            UserId = userId,
            CreditId = creditId,
            DocumentHash = documentHash,
        };
        var response = await _creditApplicationService.VerifySinatureAsync(auditAcceptance);
        return Ok(response);
    }

    /// <summary>
    /// Validación de la firma.
    /// </summary>
    /// <param name="request">Información a validar</param>
    /// <returns>Resultado de la validación de firma</returns>
    [HttpPost("Verify")]
    [ProducesResponseType(typeof(VerifySignatureDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyHashSignature([FromBody] VerifySignatureRequest request)
    {
        var auditAcceptance = new SignatureDto
        {
            UserId = request.UserId,
            CreditId = request.CreditId,
            DocumentHash = request.DocumentHash,
        };
        var response = await _creditApplicationService.VerifySinatureAsync(auditAcceptance);
        return Ok(response);
    }
}
