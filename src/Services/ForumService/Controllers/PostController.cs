using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ForumService.AsyncDataServices;
using ForumService.Data;
using ForumService.Dtos;
using ForumService.Helpers;
using ForumService.Models;

namespace ForumService.Controllers;

[ApiController]
[Route("/[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IPostRepo _repository;
    private readonly IAsyncMessageProvider _asyncMessageProvider;

    public PostController(ILogger<PostController> logger, IMapper mapper, IPostRepo repository, IAsyncMessageProvider asyncMessageProvider)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
        _asyncMessageProvider = asyncMessageProvider;
    }
    
    [HttpGet(Name = "GetPosts")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<PostReadDto>> GetPosts([FromQuery]int? userId = null, [FromQuery]int? themeId = null, [FromQuery]int? postId = null)
    {
        var postModels = _repository.GetPostsWithFilters(userId, themeId, postId);

        if (!postModels.Any())
        {
            return Empty;
        }

        var postReads = _mapper.Map<IEnumerable<PostReadDto>>(postModels);
        return Ok(postReads);
    }
    
    [HttpPost]
    [Authorize(Policy = "User")]
    public ActionResult<PostReadDto> CreatePost(PostCreateDto postCreate)
    {
        var postModel = _mapper.Map<Post>(postCreate);
        postModel.CreationTimeStamp = DateTime.UtcNow;
        postModel.UserId = UserHelper.GetUserId(HttpContext);
        
        _repository.CreatePost(postModel);
        _repository.SaveChanges();
        
        var postRead = _mapper.Map<PostReadDto>(postModel);
        
        try
        {
            var postPublishDto = _mapper.Map<PostCreatePublishDto>(postModel);
            _asyncMessageProvider.PublishPostCreated(postPublishDto);
        }
        catch (Exception ex)
        {
            _logger.LogError("--> Can't send asynchronous message: {message}", ex.Message);
        }

        return CreatedAtRoute(nameof(GetPosts), new { postRead.Id }, postRead);
    }
}