using AutoMapper;
using ForumService.Dtos;
using ForumService.Models;

namespace ForumService.Profiles;

public class ThemeProfile : Profile
{
    public ThemeProfile()
    {
        CreateMap<Theme, ThemeReadDto>();
        CreateMap<ThemeCreateDto, Theme>();
    }
}