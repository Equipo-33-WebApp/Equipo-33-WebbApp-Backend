using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

/// <summary>
/// Modelo para la tabla de documentos en Supabase
/// </summary>
[Table("documents")]
public class DocumentModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("credit_id")]
    public Guid CreditId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("url")]
    public string Url { get; set; } = string.Empty;

    [Column("uploaded_at")]
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}