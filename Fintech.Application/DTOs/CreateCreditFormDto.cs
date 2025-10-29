namespace Fintech.Application.DTOs
{
    public class CreateCreditFormDto
    {
        public Guid PymeId { get; set; }
        public decimal Amount { get; set; } = 0;
        public int TermInMonths { get; set; }
        public decimal AnnualIncome { get; set; } = 0;
        public decimal NetIncome { get; set; } = 0;
        public string CreditDestination { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public string Status { get; set; } = "pendiente";
        public string Purpose { get; set; } = string.Empty;
    }
}