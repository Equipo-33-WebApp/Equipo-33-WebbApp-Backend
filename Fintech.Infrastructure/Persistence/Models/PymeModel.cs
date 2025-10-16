using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

[Table("Pymes")]
public class PymeModel : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("auth_id")]
    public Guid AuthId { get; set; }

    [Column("company_name")]
    public string CompanyName { get; set; } = string.Empty;

    [Column("address")]
    public string Address { get; set; } = string.Empty;

    [Column("sector")]
    public string Sector { get; set; } = string.Empty;

    [Column("employees")]
    public int Employees { get; set; }

    [Column("phone")]
    public string Phone { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
