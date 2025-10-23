using AutoMapper;
using Fintech.Application.DTOs.KycValidation;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles;

public class KycProfile : Profile
{
    public KycProfile()
    {
        CreateMap<UpdateKycPymeDto, Kyc>();
        CreateMap<Kyc, PymeKycModel>().ReverseMap();
        CreateMap<UpdateKycPymeDto, Kyc>();
    }
}
