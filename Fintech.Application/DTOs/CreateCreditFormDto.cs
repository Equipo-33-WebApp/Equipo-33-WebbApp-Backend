namespace Fintech.Application.DTOs
{
    public class CreateCreditFormDto
    {
        public Guid PymeId { get; set; }
        public decimal Amount { get; set; }= 0;
        public string Purpose { get; set; } = string.Empty;
        public string Status { get; set; } = "pendiente";
    }
}