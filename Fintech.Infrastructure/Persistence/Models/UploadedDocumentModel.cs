using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

/// <summary>
/// Modelo para la tabla de uploaded_documents en Supabase
/// </summary>
[Table("uploaded_documents")]
public class UploadedDocumentModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("credit_form_id")]
    public Guid CreditFormId { get; set; }

    [Column("file_url")]
    public string FileUrl { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}