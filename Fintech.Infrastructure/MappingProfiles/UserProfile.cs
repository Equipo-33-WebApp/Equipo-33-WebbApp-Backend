using AutoMapper;
using Fintech.Application.DTOs;
using Fintech.Domain.Entities;
using Fintech.Infrastructure.Persistence.Models;

namespace Fintech.Infrastructure.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Created_At, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<UpdateUserDto, User>();
    }
}