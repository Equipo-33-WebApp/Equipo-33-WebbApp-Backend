using Fintech.Application.DTOs.Aml;

namespace Fintech.Application.Interfaces.Aml;

public interface IGeminiAmlService
{
    Task<GeminiAmlResponseDto> AnalyzeAsync(GeminiAmlRequestDto req);
}