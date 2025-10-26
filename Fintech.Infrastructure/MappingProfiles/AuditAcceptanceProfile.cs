using AutoMapper;
using Fintech.Application.DTOs.DigitalSignature;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using Fintech.Infrastructure.Utils;

namespace Fintech.Infrastructure.MappingProfiles;

public class AuditAcceptanceProfile : Profile
{
    public AuditAcceptanceProfile()
    {
        CreateMap<AuditAcceptanceDto, AuditAcceptance>()
            .ForMember(dest => dest.DocumentHash, opt =>
                opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.DocumentHash)
                        ? HashHelper.ComputeSha256(src.DocumentText)
                        : src.DocumentHash
                ));
        CreateMap<AuditAcceptance, AuditAcceptanceDto>();


        CreateMap<SignatureDto, AuditAcceptance>()
            .ForMember(dest => dest.DocumentHash, opt =>
                opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.DocumentHash)
                        ? HashHelper.ComputeSha256(src.DocumentText)
                        : src.DocumentHash
                ));
        CreateMap<AuditAcceptance, SignatureDto>();

        CreateMap<AuditAcceptance, AuditAcceptanceModel>().ReverseMap();

        CreateMap<AuditDocumentDto, AuditAcceptance>();
        CreateMap<AuditAcceptance, AuditDocumentDto>();
    }
}
