using AutoMapper;
using Fintech.Domain.Entities.Panel;
using Fintech.Infrastructure.Persistence.Models.Panel;

namespace Fintech.Infrastructure.MappingProfiles;

public class PanelProfile : Profile
{
    public PanelProfile()
    {
        CreateMap<CreditApplicationPanel, CreditApplicationPanelModel>().ReverseMap()
            .ForMember(dest => dest.PymeId, opt => opt.MapFrom(src => src.Pyme.Id))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Pyme.CompanyName))
            .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.Pyme.Sector));
    }
}