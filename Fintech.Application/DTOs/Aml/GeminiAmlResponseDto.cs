using System.Text.Json.Serialization;

namespace Fintech.Application.DTOs.Aml;

public class GeminiAmlResponseDto
{
    [JsonPropertyName("riskLevel")]
    public string RiskLevel { get; set; } = string.Empty;

    [JsonPropertyName("flags")]
    public string[] Flags { get; set; } = Array.Empty<string>();

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("requiresManualReview")]
    public bool RequiresManualReview { get; set; }
}