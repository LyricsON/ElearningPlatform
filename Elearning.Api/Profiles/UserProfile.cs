using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserDto>();

        CreateMap<CreateUserDto, AppUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

        CreateMap<UpdateUserDto, AppUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}
