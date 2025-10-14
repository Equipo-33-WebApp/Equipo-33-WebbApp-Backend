namespace Fintech.Domain.Entities;

public class Pyme : Entity
{
    public Guid UserId { get; set; }         
    public string CompanyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public int Employees { get; set; }        
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
