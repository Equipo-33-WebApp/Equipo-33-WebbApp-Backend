namespace Fintech.Domain.Entities;

public class AuditAcceptance : Entity
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentHash { get; set; } = string.Empty;
    public string ConsentType { get; set; } = string.Empty;
    public string AcceptanceType { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}
