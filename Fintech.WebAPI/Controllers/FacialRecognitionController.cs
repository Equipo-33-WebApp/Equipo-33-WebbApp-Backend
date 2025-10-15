using Microsoft.AspNetCore.Mvc;
using Fintech.Application.Interfaces;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class FacialRecognitionController : ControllerBase
{
    private readonly IFacialRecognitionService _deepFaceService;

    public FacialRecognitionController(IFacialRecognitionService deepFaceService)
    {
        _deepFaceService = deepFaceService;
    }

    /// <summary>
    /// Verifica si dos imágenes son similares.
    /// </summary>
    /// <param name="img1">Archivo de imagen 1</param>
    /// <param name="img2">Archivo de imagen 2</param>
    /// <param name="modelName">Nombre del modelo de reconocimiento facial (default: ArcFace)</param>
    /// <param name="detectorBackend">Detenctor de caras(default: retinaface)</param>
    /// <returns>Resultado de la comparación</returns>
    [HttpPost("verify")]
    public async Task<IActionResult> Verify(
        IFormFile img1,
        IFormFile img2,
        [FromForm] string modelName = "ArcFace",
        [FromForm] string detectorBackend = "retinaface")
    {
        if (img1 == null || img2 == null)
        {
            return BadRequest("Both img1 and img2 are required.");
        }

        var result = await _deepFaceService.Verify(
            img1.OpenReadStream(),
            img1.FileName,
            img1.ContentType,
            img2.OpenReadStream(),
            img2.FileName,
            img2.ContentType,
            modelName,
            detectorBackend);
        return Ok(result);
    }

    /// <summary>
    /// Extrae la cara de una imagen.
    /// </summary>
    /// <param name="idDocument">Archivo de imagen del documento</param>
    /// <param name="detectorBackend">Detenctor de caras(default: retinaface)</param>
    /// <returns>Imagen de la cara extraída</returns>
    [HttpPost("extract_face")]
    public async Task<IActionResult> ExtractFace(
        IFormFile idDocument,
        [FromForm] string detectorBackend = "retinaface")
    {
        if (idDocument == null)
        {
            return BadRequest("id_document is required.");
        }

        var result = await _deepFaceService.ExtractFace(
            idDocument.OpenReadStream(),
            idDocument.FileName,
            idDocument.ContentType,
            detectorBackend
        );

        if (!string.IsNullOrEmpty(result.Error))
        {
            return BadRequest(result.Error);
        }

        if (result.FaceImage != null)
        {
            return File(result.FaceImage, "image/png", "face.png");
        }

        return StatusCode(500, "An unexpected error occurred.");
    }

    /// <summary>
    /// Verificación de documento y selfie.
    /// </summary>
    /// <param name="idDocument">Archivo de imagen del documento</param>
    /// <param name="selfie">Archivo de imagen del selfie</param>
    /// <param name="modelName"></param>
    /// <param name="detectorBackend"></param>
    /// <returns></returns>
    [HttpPost("verify_face")]
    public async Task<IActionResult> VerifyDocumentAndSelfie(
        IFormFile idDocument,
        IFormFile selfie,
        [FromForm] string modelName = "ArcFace",
        [FromForm] string detectorBackend = "retinaface")
    {
        if (idDocument == null || selfie == null)
        {
            return BadRequest("Both idDocument and selfie are required.");
        }

        var result = await _deepFaceService.VerifyDocumentAndSelfie(
            idDocument.OpenReadStream(),
            idDocument.FileName,
            idDocument.ContentType,
            selfie.OpenReadStream(),
            selfie.FileName,
            selfie.ContentType,
            modelName,
            detectorBackend
        );

        if (!string.IsNullOrEmpty(result.Error))
        {
            return BadRequest(result.Error);
        }

        return Ok(result.VerificationResult);
    }

    /// <summary>
    /// Redimensiona una imagen.
    /// </summary>
    /// <param name="img">Archivo de imagen</param>
    /// <returns>Imagen redimensionada</returns>
    [HttpPost("resize_image")]
    public async Task<IActionResult> ResizeImage(IFormFile img)
    {
        if (img == null || img.Length == 0)
        {
            return BadRequest("Image file is required.");
        }

        var result = await _deepFaceService.ResizeImage(img.OpenReadStream(), img.FileName, img.ContentType);

        if (result.Error != null)
        {
            return StatusCode(500, result.Error);
        }

        if (result.ResizedImage == null || result.ContentType == null || result.FileName == null)
        {
            return StatusCode(500, "Failed to resize image.");
        }

        return File(result.ResizedImage, result.ContentType, result.FileName);
    }
}
