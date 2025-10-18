using Fintech.Application.DTOs.KycValidation;
using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KycVerificationController : ControllerBase
{
    private readonly IKycVerificationService _documentVerificationService;

    public KycVerificationController(IKycVerificationService documentVerificationService)
    {
        _documentVerificationService = documentVerificationService;
    }

    /// <summary>
    /// Verificación de documento y selfie.
    /// </summary>
    /// <param name="nationalIdNumber">Número de identificación nacional</param>
    /// <param name="idDocumentFront">Archivo de imagen del documento</param>
    /// <param name="faceSelfie">Archivo de imagen del selfie</param>
    /// <returns>Resultado de la verificación</returns>
    [HttpPost("verify_identity")]
    [ProducesResponseType(typeof(KycVerificationResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyIdDocumentAndSelfie(
        [FromForm] string nationalIdNumber,
        IFormFile idDocumentFront,
        IFormFile faceSelfie)
    {
        if (string.IsNullOrEmpty(nationalIdNumber) || idDocumentFront == null || faceSelfie == null)
        {
            return BadRequest("NationalIdNumber, IdDocumentFront, and FaceSelfie are required.");
        }

        using var idDocumentMemoryStream = new MemoryStream();
        await idDocumentFront.CopyToAsync(idDocumentMemoryStream);
        idDocumentMemoryStream.Position = 0; 

        using var faceSelfieMemoryStream = new MemoryStream();
        await faceSelfie.CopyToAsync(faceSelfieMemoryStream);
        faceSelfieMemoryStream.Position = 0; 

        var requestDto = new KycVerificationRequestDto
        {
            NationalIdNumber = nationalIdNumber,
            IdDocumentFront = idDocumentMemoryStream,
            IdDocumentFrontName = idDocumentFront.FileName,
            IdDocumentFrontContentType = idDocumentFront.ContentType,
            FaceSelfie = faceSelfieMemoryStream,
            FaceSelfieName = faceSelfie.FileName,
            FaceSelfieContentType = faceSelfie.ContentType
        };

        var result = await _documentVerificationService.VerifyDocumentAndSelfieAndExtractData(requestDto);

        return Ok(result);
    }
}
