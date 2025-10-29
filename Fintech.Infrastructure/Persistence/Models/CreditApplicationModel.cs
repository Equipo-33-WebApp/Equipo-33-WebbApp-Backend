using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

/// <summary>
/// Modelo para la tabla de solicitud de créditos en Supabase
/// </summary>
[Table("credit_applications")]
public class CreditApplicationModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("purpose")]
    public string Purpose { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "Draft";

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}