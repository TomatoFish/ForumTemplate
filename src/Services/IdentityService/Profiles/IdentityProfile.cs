using AutoMapper;
using IdentityService.Dtos;
using IdentityService.Models;

namespace IdentityService.Profiles;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<RegistrationRequest, User>();
        CreateMap<RegistrationRequest, AuthenticationRequest>()
            .ForMember(auth => auth.Login, opt => opt.MapFrom(u => u.Username))
            .ForMember(auth => auth.Password, opt => opt.MapFrom(u => u.Password))
            .ForMember(auth => auth.RememberLogin, opt => opt.Ignore());
    }
}