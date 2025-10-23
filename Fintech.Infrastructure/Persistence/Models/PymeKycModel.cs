using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

[Table("pymes")]
public class PymeKycModel : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("auth_id")]
    public Guid AuthId { get; set; }

    [Column("has_kyc_validated")]
    public bool HasKycValidated { get; set; }
}
