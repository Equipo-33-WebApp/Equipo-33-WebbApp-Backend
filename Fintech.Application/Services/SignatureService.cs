using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Signature;
using Fintech.Application.Interfaces.CreditApplication;
using Fintech.Domain.Entities;

namespace Fintech.Application.Services;

public class SignatureService(ISignatureRepository _signatureRepository, IMapper _mapper) : ISignatureService
{
    public const string INVALID_SIGNATURE = "La firma es inválida";
    public const string VALID_SIGNATURE = "La firma es válida";
    public const string INVALID_TEXT = "El texto ingresado es inválido";
    public const string INVALID_CREDITID = "El credito ingresado es inválido";
    public const string INVALID_USERID = "El usuario ingresado es inválido";

    public async Task<SignAgreementDto> SignAgreementAsync(AuditAcceptanceDto auditAcceptanceDto)
    {
        if (string.IsNullOrEmpty(auditAcceptanceDto.DocumentText)) return new SignAgreementDto() { Message = INVALID_TEXT };
        if (auditAcceptanceDto.CreditId == Guid.Empty) return new SignAgreementDto() { Message = INVALID_CREDITID };
        if (auditAcceptanceDto.UserId == Guid.Empty) return new SignAgreementDto() { Message = INVALID_USERID };

        var auditAcceptance = _mapper.Map<AuditAcceptance>(auditAcceptanceDto);
        var creditApp = await _signatureRepository.CreateAsync(auditAcceptance);
        if (creditApp == null)
        {
            return null;
        }
        var result = _mapper.Map<AuditAcceptanceDto>(creditApp);
        result.DocumentText = auditAcceptanceDto.DocumentText;
        return new SignAgreementDto()
        {
            Message = "OK",
            Signature = result,
        };
    }

    public async Task<VerifySignatureDto> VerifySignatureAsync(SignatureDto signatureDto)
    {
        if (string.IsNullOrEmpty(signatureDto.DocumentText)) return new VerifySignatureDto() { Message = INVALID_TEXT };
        if (signatureDto.CreditId == Guid.Empty) return new VerifySignatureDto() { Message = INVALID_CREDITID };
        if (signatureDto.UserId == Guid.Empty) return new VerifySignatureDto() { Message = INVALID_USERID };

        var auditAcceptance = _mapper.Map<AuditAcceptance>(signatureDto);
        var creditApp = await _signatureRepository.GetAsync(auditAcceptance);
        if (creditApp == null)
        {
            return new VerifySignatureDto() { Message = INVALID_SIGNATURE };
        }
        var auditSignature = _mapper.Map<SignatureDto>(creditApp);
        auditSignature.DocumentText = signatureDto.DocumentText;
        return new VerifySignatureDto()
        {
            IsValid = true,
            Message = VALID_SIGNATURE,
            Signature = auditSignature,
        };
    }
}
