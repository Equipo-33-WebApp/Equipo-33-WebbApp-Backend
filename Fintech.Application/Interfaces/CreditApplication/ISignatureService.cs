using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Signature;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface ISignatureService
{
    Task<SignAgreementDto> SignTextSignatureAsync(AuditAcceptanceDto auditAcceptanceDto);
    Task<VerifySignatureDto> VerifyTextSignatureAsync(SignatureDto signatureDto);

    Task<SignDocumentDto> SignDocumentAsync(AuditDocumentDto auditDocumentDto);
    Task<VerifySignatureDto> VerifySinatureAsync(SignatureDto signatureDto);
}
