namespace Fintech.Application.DTOs.Aml;

public class AmlResultDto
{
    public double RiskLevel { get; set; }
    public string ResultSummary { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}