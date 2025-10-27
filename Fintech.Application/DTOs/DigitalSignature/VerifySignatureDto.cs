namespace Fintech.Application.DTOs.DigitalSignature;

public class VerifyTextSignatureRequest
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentText { get; set; } = string.Empty;
}


public class SignatureDto
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentText { get; set; } = string.Empty;
    public string DocumentHash { get; set; } = string.Empty;
    public string ConsentType { get; set; } = string.Empty;
    public string AcceptanceType { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}

public class VerifySignatureDto
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public SignatureDto? Signature { get; set; }
}

public class VerifySignatureRequest
{
    public Guid CreditId { get; set; }
    public Guid UserId { get; set; }
    public string DocumentHash { get; set; } = string.Empty;
}