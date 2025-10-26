using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles;

public class PymeProfile : Profile
{
    public PymeProfile()
    {
        CreateMap<PymeRequestDto, Pyme>();
        CreateMap<Pyme, PymeModel>().ReverseMap();
        CreateMap<UpdatePymeDto, Pyme>();
    }
}
