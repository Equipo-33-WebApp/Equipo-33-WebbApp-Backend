using Fintech.Application.DTOs.KycValidation;

namespace Fintech.Application.Interfaces;

public interface ITextRecognitionService
{
    Task<RecognizedTextDto> ParseImageAsync(Stream file, string fileName, string contentType);
}
