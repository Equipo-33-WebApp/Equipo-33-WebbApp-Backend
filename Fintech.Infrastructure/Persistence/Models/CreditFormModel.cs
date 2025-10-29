using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace Fintech.Infrastructure.Persistence.Models;

/// <summary>
/// Modelo para la tabla de solicitud de créditos en Supabase
/// </summary>
[Table("credit_forms")]
public class CreditFormModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }
    [Column("pyme_id")]
    public Guid PymeId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("purpose")]
    public string Purpose { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "draft";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    [JsonPropertyName("uploaded_documents")]
    public List<UploadedDocumentModel> UploadedDocuments { get; set; } = new();
}