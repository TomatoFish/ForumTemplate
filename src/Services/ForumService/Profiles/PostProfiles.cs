using AutoMapper;
using ForumService.Dtos;
using ForumService.Models;

namespace ForumService.Profiles;

public class PostProfiles : Profile
{
    public PostProfiles()
    {
        CreateMap<Post, PostReadDto>();//.ForMember(postRead => postRead.ThemeId, opt => opt.MapFrom(post => post.Theme != null ? post.Theme.Id : 0));
        CreateMap<PostCreateDto, Post>();
        CreateMap<Post, PostCreatePublishDto>();
    }
}