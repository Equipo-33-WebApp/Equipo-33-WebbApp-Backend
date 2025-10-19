namespace Fintech.Application.DTOs.Signature;

public class AuditDocumentDto
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentHash { get; set; } = string.Empty;
    public string ConsentType { get; set; } = "CreditAcceptance";
    public string AcceptanceType { get; set; } = "ScannedSignature";
    public string Host { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}

public class SignDocumentDto
{
    public string Message { get; set; } = string.Empty;
    public AuditDocumentDto? Signature { get; set; }
}
