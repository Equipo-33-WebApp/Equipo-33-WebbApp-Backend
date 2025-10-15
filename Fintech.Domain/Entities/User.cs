namespace Fintech.Domain.Entities;

public class User : Entity
{
    public string FullName { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "PYME";
    public DateTime Created_At { get; set; } = DateTime.UtcNow;
}