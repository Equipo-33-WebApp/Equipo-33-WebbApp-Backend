namespace Fintech.Domain.Entities;

public class AmlCheck: Entity
{
    public Guid AuthId { get; set; }
    public Guid PymeId { get; set; }
    public string RiskLevel { get; set; } = "Unknown";
    public List<string> Flags { get; set; } = new List<string>();
    public string ResultSummary { get; set; } = string.Empty;
    public bool RequiresManualReview { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}