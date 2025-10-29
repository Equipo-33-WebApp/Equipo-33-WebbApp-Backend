using System.ComponentModel.DataAnnotations.Schema;

namespace Fintech.Domain.Entities
{
    public class CreditForm : Entity
    {
        public Guid UserId { get; set; }
        public Guid PymeId { get; set; }

        public decimal? Amount { get; set; }
        public int TermInMonths { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal NetIncome { get; set; }
        public string CreditDestination { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public string? Purpose { get; set; }
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<UploadedDocument>? UploadedDocuments { get; set; } = new();
    }
}