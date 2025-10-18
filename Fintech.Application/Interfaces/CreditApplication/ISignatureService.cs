using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Signature;

namespace Fintech.Application.Interfaces.CreditApplication;

public interface ISignatureService
{
    Task<SignAgreementDto> SignAgreementAsync(AuditAcceptanceDto auditAcceptanceDto);
    Task<VerifySignatureDto> VerifySignatureAsync(SignatureDto signatureDto);
}
