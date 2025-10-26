namespace Fintech.Domain.Entities
{
    public class CreditForm : Entity
    {
        public Guid UserId { get; set; }
        public Guid PymeId { get; set; }

        public decimal? Amount { get; set; }
        public string? Purpose { get; set; }
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<UploadedDocument>? UploadedDocuments { get; set; } = new();
    }
}