namespace Fintech.Domain.Entities
{
    public class UploadedDocument
    {
        public Guid Id { get; set; }
        public Guid CreditFormId { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}