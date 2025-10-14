using Fintech.Application.DTOs.KycValidation;
using Refit;

namespace Fintech.Infrastructure.Interfaces;

public interface IOcrSpaceApi
{
    [Multipart]
    [Post("/parse/image")]
    Task<RecognizedTextDto> ParseImageAsync(
        [AliasAs("apikey")] string apiKey,
        [AliasAs("language")] string language,
        [AliasAs("isOverlayRequired")] bool isOverlayRequired,
        [AliasAs("file")] StreamPart file);
}
