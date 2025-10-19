namespace Fintech.Application.DTOs.Signature;

public class SignAgreementRequest
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentText { get; set; } = string.Empty;
}


public class AuditAcceptanceDto
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentText { get; set; } = string.Empty;
    public string DocumentHash { get; set; } = string.Empty;
    public string ConsentType { get; set; } = "CreditTerms";
    public string AcceptanceType { get; set; } = "ClickWrap";
    public string Host { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}

public class SignAgreementDto
{
    public string Message { get; set; } = string.Empty;
    public AuditAcceptanceDto? Signature { get; set; }
}
