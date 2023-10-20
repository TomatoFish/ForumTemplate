using AutoMapper;
using ForumService.Dtos;
using ForumService.Models;

namespace ForumService.Profiles;

public class CommentProfiles : Profile
{
    public CommentProfiles()
    {
        CreateMap<Comment, CommentReadDto>();//.ForMember(commentRead => commentRead.PostId, opt => opt.MapFrom(comment => comment.Post.Id));
        CreateMap<CommentCreateDto, Comment>();
    }
}