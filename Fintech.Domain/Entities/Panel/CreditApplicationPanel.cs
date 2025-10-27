namespace Fintech.Domain.Entities.Panel;

public class CreditApplicationPanel : Entity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }


    public Guid PymeId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
}
