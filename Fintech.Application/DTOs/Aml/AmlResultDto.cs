namespace Fintech.Application.DTOs.Aml;

public class AmlResultDto
{
    public Guid Id { get; set; }
    public string RiskLevel { get; set; } = "Unknown";
    public List<string> Flags { get; set; } = [];
    public string ResultSummary { get; set; } = string.Empty;
    public bool RequiresManualReview { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}