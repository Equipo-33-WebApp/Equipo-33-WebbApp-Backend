using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Application.DTOs.Signature;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;
using Fintech.Infrastructure.Utils;

namespace Fintech.Infrastructure.MappingProfiles;

public class AuditAcceptanceProfile : Profile
{
    public AuditAcceptanceProfile()
    {
        CreateMap<AuditAcceptanceDto, AuditAcceptance>()
            .ForMember(dest => dest.DocumentHash, opt => opt.MapFrom(opt => HashHelper.ComputeSha256(opt.DocumentText)));
        CreateMap<AuditAcceptance, AuditAcceptanceDto>();


        CreateMap<SignatureDto, AuditAcceptance>()
            .ForMember(dest => dest.DocumentHash, opt => opt.MapFrom(opt => HashHelper.ComputeSha256(opt.DocumentText)));
        CreateMap<AuditAcceptance, SignatureDto>();

        CreateMap<AuditAcceptance, AuditAcceptanceModel>().ReverseMap();
    }
}
