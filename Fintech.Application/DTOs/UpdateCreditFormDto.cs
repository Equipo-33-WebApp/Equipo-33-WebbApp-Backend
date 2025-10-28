namespace Fintech.Application.DTOs
{
    public class UpdateCreditFormDto
    {
        public decimal Amount { get; set; }= 0;
        public string Purpose { get; set; } = string.Empty;
        public string Status { get; set; } = "pendiente";
    }
}