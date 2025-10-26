using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

[Table("pymes")]
public class PymeKycModel : BaseModel
{
    [PrimaryKey("auth_id")]
    public Guid AuthId { get; set; }

    [Column("has_kyc_validated")]
    public bool HasKycValidated { get; set; }

    [Column("national_number")]
    public string NationalIdNumber { get; set; } = string.Empty;

    [Column("document_front")]
    public string DocumentFrontHash { get; set; } = string.Empty;

    [Column("face_selfie")]
    public string FaceSelfieHash { get; set; } = string.Empty;
}
