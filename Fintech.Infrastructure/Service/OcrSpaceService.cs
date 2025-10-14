using Fintech.Application.DTOs.KycValidation;
using Fintech.Application.Interfaces;
using Fintech.Infrastructure.Interfaces;
using Refit;

namespace Fintech.Infrastructure.Service;

public class OcrSpaceService : ITextRecognitionService
{
    private readonly IOcrSpaceApi _ocrSpaceApi;
    private readonly string _ocrSpaceApiKey;

    public OcrSpaceService(IOcrSpaceApi ocrSpaceApi)
    {
        _ocrSpaceApi = ocrSpaceApi;
        _ocrSpaceApiKey = Environment.GetEnvironmentVariable("OCRSPACE_API_KEY") ?? throw new Exception("OCRSPACE_API_KEY environment variable is not set.");
    }

    public async Task<RecognizedTextDto> ParseImageAsync(Stream file, string fileName, string img1ContentType)
    {
        var stream1 = new StreamPart(file, fileName, img1ContentType);
        var response = await _ocrSpaceApi.ParseImageAsync(
            apiKey: _ocrSpaceApiKey,
            language: "eng",
            isOverlayRequired: false,
            file: stream1
        );

        return response;
    }
}
