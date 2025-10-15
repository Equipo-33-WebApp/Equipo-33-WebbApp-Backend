using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Fintech.Infrastructure.Persistence.Models;

/// <summary>
/// Modelo para la tabla de usuarios en Supabase
/// </summary>
[Table("users")]
public class UserModel : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("auth_id")]
    public Guid AuthId { get; set; }

    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("role")]
    public string Role { get; set; } = "PYME";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}