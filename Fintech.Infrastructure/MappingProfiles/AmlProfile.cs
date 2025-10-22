using AutoMapper;
using Fintech.Application.DTOs.Aml;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles;

public class AmlProfile : Profile
{
    public AmlProfile()
    {
        CreateMap<AmlCheck, AmlCheckModel>().ReverseMap();

        CreateMap<AmlCheck, AmlResultDto>();
    }
}