using Fintech.Application.DTOs.KycValidation;

namespace Fintech.Application.Interfaces;

public interface IKycVerificationService
{
    Task<KycVerificationResultDto> VerifyDocumentAndSelfieAndExtractData(
        KycVerificationRequestDto request);
}
