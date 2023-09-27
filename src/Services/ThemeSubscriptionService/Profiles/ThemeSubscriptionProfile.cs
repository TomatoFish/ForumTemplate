using AutoMapper;
using ThemeSubscriptionService.Dtos;
using ThemeSubscriptionService.Models;

namespace ThemeSubscriptionService.Profiles;

public class ThemeSubscriptionProfile : Profile
{
    public ThemeSubscriptionProfile()
    {
        CreateMap<ThemeSubscriptionCreateDto, ThemeSubscription>();
        CreateMap<ThemeSubscriptionRemoveDto, ThemeSubscription>();
        CreateMap<ThemeSubscription, ThemeSubscriptionReadDto>();
    }
}