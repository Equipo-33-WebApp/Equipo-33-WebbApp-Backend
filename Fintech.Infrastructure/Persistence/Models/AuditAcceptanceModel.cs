using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

[Table("audit_acceptances")]
public class AuditAcceptanceModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("credit_id")]
    public Guid CreditId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("document_hash")]
    public string DocumentHash { get; set; } = string.Empty;

    [Column("consent_type")]
    public string ConsentType { get; set; } = string.Empty;

    [Column("acceptance_type")]
    public string AcceptanceType { get; set; } = string.Empty;

    [Column("host")]
    public string Host { get; set; } = string.Empty;

    [Column("user_agent")]
    public string UserAgent { get; set; } = string.Empty;

    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
