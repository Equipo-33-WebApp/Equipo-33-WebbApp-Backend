namespace Fintech.Domain.Entities;

public class AmlCheck: Entity
{
    public Guid AuthId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = "Unknown";
    public string ResultSummary { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}