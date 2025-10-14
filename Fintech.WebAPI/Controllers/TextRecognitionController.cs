using Fintech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class TextRecognitionController : ControllerBase
{
    private readonly ITextRecognitionService _ocrSpaceService;
    private readonly IFacialRecognitionService _deepFaceService;

    public TextRecognitionController(ITextRecognitionService ocrSpaceService, IFacialRecognitionService deepFaceService)
    {
        _ocrSpaceService = ocrSpaceService;
        _deepFaceService = deepFaceService;
    }

    [HttpPost("extract_text")]
    public async Task<IActionResult> ParseImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        var resizeResult = await _deepFaceService.ResizeImage(stream, file.FileName, file.ContentType);

        if (resizeResult.Error != null)
        {
            return StatusCode(500, resizeResult.Error);
        }

        if (resizeResult.ResizedImage == null || resizeResult.ContentType == null || resizeResult.FileName == null)
        {
            return StatusCode(500, "Failed to resize image.");
        }

        using var resizedStream = new MemoryStream(resizeResult.ResizedImage);
        var response = await _ocrSpaceService.ParseImageAsync(resizedStream, resizeResult.FileName, resizeResult.ContentType);

        if (response.IsErroredOnProcessing)
            return BadRequest(response.ErrorMessage);

        return Ok(response.ParsedResults);
    }
}
