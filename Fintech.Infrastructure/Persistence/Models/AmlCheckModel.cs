using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

/// <summary>
/// Modelo para la tabla de verificación AML en Supabase
/// </summary>
[Table("aml_checks")]
public class AmlCheckModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("auth_id")]
    public Guid AuthId { get; set; }

    [Column("pyme_id")]
    public Guid PymeId { get; set; }

    [Column("flags")]
    public List<string> Flags { get; set; } = new List<string>();

    [Column("risk_level")]
    public string RiskLevel { get; set; } = "Unknown";

    [Column("result_summary")]
    public string ResultSummary { get; set; } = string.Empty;

    [Column("requires_manual_review")]
    public bool RequiresManualReview { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}