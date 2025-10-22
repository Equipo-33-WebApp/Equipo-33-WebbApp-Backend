using AutoMapper;
using Fintech.Application.DTOs.Aml;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles;

public class AmlProfile : Profile
{
    public AmlProfile()
    {
        CreateMap<AmlRequestDto, AmlCheckModel>()
            .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber ?? ""));

        CreateMap<AmlCheck, AmlCheckModel>().ReverseMap();

        CreateMap<AmlCheck, AmlResultDto>();
    }
}