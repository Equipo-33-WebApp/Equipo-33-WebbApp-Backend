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
