using AutoMapper;
using ForumService.Data;
using ForumService.Dtos;
using ForumService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumService.Controllers;

[ApiController]
[Route("/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICommentRepo _repository;

    public CommentController(ILogger<PostController> logger, IMapper mapper, ICommentRepo repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet(Name = "GetComments")]
    public ActionResult<IEnumerable<CommentReadDto>> GetComments([FromQuery]int? postId = null, [FromQuery]int? userId = null, [FromQuery]int? parentCommentId = null, [FromQuery]int? commentId = null)
    {
        var commentModels = _repository.GetCommentsWithFilters(postId, userId, parentCommentId, commentId);

        if (!commentModels.Any())
        {
            return Empty;
        }

        var commentReads = _mapper.Map<IEnumerable<CommentReadDto>>(commentModels);
        return Ok(commentReads);
    }
    
    [HttpPost]
    [Authorize(Policy = "User")]
    public ActionResult<CommentReadDto> CreateComment(CommentCreateDto commentCreate)
    {
        var commentModel = _mapper.Map<Comment>(commentCreate);
        commentModel.CreationTimeStamp = DateTime.UtcNow;
        
        _repository.CreateComment(commentModel);
        _repository.SaveChanges();
        
        var postRead = _mapper.Map<PostReadDto>(commentModel);
        
        return CreatedAtRoute(nameof(GetComments), new { postRead.Id }, postRead);
    }
}