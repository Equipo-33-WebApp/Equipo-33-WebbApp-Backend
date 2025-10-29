using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fintech.Application.DTOs
{
    public class UpdateCreditFormDto
    {
        public decimal Amount { get; set; }= 0;
        public int TermInMonths { get; set; }
        public decimal AnnualIncome { get; set; }= 0;
        public decimal NetIncome { get; set; }= 0;
        [DefaultValue("Capital de Trabajo")]
        public string CreditDestination { get; set; } = string.Empty;
        [DefaultValue("High")]
        [AllowedValues("Low", "Medium", "High")]
        public string RiskLevel { get; set; } = string.Empty;
        [DefaultValue("Draft")]
        [AllowedValues("Draft", "Pending")]
        public string Status { get; set; } = "Draft";
        [DefaultValue("Capital de Trabajo")]
        public string Purpose { get; set; } = string.Empty;
    }
}