namespace Fintech.Domain.Entities.Panel;

public class CreditApplicationPanel : Entity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public decimal AnnualIncome { get; set; }
    public decimal NetIncome { get; set; }
    public string CreditDestination { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public Guid PymeId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;

    public List<UploadedDocument>? UploadedDocuments { get; set; } = new();
}
