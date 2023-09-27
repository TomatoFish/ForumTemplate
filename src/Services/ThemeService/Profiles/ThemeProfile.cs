using AutoMapper;
using ThemeService.Dtos;
using ThemeService.Models;

namespace ThemeService.Profiles;

public class ThemeProfile : Profile
{
    public ThemeProfile()
    {
        CreateMap<Theme, ThemeReadDto>();
        CreateMap<ThemeCreateDto, Theme>();
    }
}