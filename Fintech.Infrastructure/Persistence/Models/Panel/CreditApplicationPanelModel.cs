using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace Fintech.Infrastructure.Persistence.Models.Panel;

[Table("credit_forms")]
public class CreditApplicationPanelModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }
    [Column("pyme_id")]
    public Guid PymeId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("term_in_months")]
    public int TermInMonths { get; set; }
    [Column("annual_income")]
    public decimal AnnualIncome { get; set; }
    [Column("net_income")]
    public decimal NetIncome { get; set; }
    [Column("credit_destination")]
    public string CreditDestination { get; set; } = string.Empty;

    [Column("risk_level")]
    public string RiskLevel { get; set; } = string.Empty;

    [Column("purpose")]
    public string Purpose { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "Draft";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Reference(typeof(PymePanelModel), ReferenceAttribute.JoinType.Left)]
    public PymePanelModel Pyme { get; set; }

    [Reference(typeof(UploadedDocumentModel), ReferenceAttribute.JoinType.Left)]
    public List<UploadedDocumentModel> UploadedDocuments { get; set; } = new();
}
