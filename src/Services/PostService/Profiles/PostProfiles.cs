using AutoMapper;
using PostService.Dtos;
using PostService.Models;

namespace PostService.Profiles;

public class PostProfiles : Profile
{
    public PostProfiles()
    {
        CreateMap<Post, PostReadDto>();
        CreateMap<PostCreateDto, Post>();
        CreateMap<Post, PostCreatePublishDto>();
    }
}