using AutoMapper;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles
{
    public class CreditFormProfile : Profile
    {
        public CreditFormProfile()
        {
            CreateMap<CreditForm, CreditFormModel>().ReverseMap();
            CreateMap<CreditForm, CreditFormModelCreate>().ReverseMap();
        }
    }
}