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

    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Column("document_number")]
    public string? DocumentNumber { get; set; } = string.Empty;

    [Column("country")]
    public string Country { get; set; } = string.Empty;

    [Column("risk_level")]
    public string RiskLevel { get; set; } = "Unknown";

    [Column("result_summary")]
    public string ResultSummary { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}