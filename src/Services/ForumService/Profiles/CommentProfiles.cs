using AutoMapper;
using ForumService.Dtos;
using ForumService.Models;

namespace ForumService.Profiles;

public class CommentProfiles : Profile
{
    public CommentProfiles()
    {
        CreateMap<Comment, CommentReadDto>();
        CreateMap<CommentCreateDto, Comment>();
    }
}