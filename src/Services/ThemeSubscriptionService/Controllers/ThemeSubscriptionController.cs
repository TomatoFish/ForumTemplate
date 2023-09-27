using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThemeSubscriptionService.Data;
using ThemeSubscriptionService.Dtos;
using ThemeSubscriptionService.Models;

namespace ThemeSubscriptionService.Controllers;

[ApiController]
[Route("[controller]")]
public class ThemeSubscriptionController : ControllerBase
{
    private readonly IThemeSubscriptionRepo _repository;
    private readonly IMapper _mapper;

    public ThemeSubscriptionController(IThemeSubscriptionRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Policy = "User")]
    public ActionResult<ThemeSubscriptionReadDto> SubscribeToTheme(ThemeSubscriptionCreateDto subscriptionCreate)
    {
        var id = GetUserId();
        
        if (_repository.IsUserSubscribedToTheme(id, subscriptionCreate.ThemeId))
        {
            return Empty;
        }
        
        var subscription = _mapper.Map<ThemeSubscription>(subscriptionCreate);
        subscription.UserId = id;
        _repository.CreateThemeSubscription(subscription);
        _repository.SaveChanges();
        var subscriberRead = _mapper.Map<ThemeSubscriptionReadDto>(subscription);
        return Ok(subscriberRead);
    }

    [HttpDelete]
    [Authorize(Policy = "User")]
    public ActionResult UnsubscribeFromTheme(ThemeSubscriptionRemoveDto subscriptionRemove)
    {
        var id = GetUserId();
        
        if (!_repository.IsUserSubscribedToTheme(id, subscriptionRemove.ThemeId))
        {
            return Empty;
        }
        
        var subscription = _mapper.Map<ThemeSubscription>(subscriptionRemove);
        _repository.RemoveThemeSubscription(subscription);
        return Ok();
    }
    
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<IEnumerable<ThemeSubscriptionReadDto>> GetAllSubscriptions()
    {
        var subscriptions = _repository.GetAllThemeSubscriptions();
        var subscribersRead = _mapper.Map<IEnumerable<ThemeSubscriptionReadDto>>(subscriptions);
        return Ok(subscribersRead);
    }
    
    [HttpGet("User={userId}")]
    [Authorize(Policy = "User")]
    public ActionResult<IEnumerable<ThemeSubscriptionReadDto>> GetSubscriptionsForUser(long userId)
    {
        var identityClaims = GetUserClaims();
        var id = identityClaims.FirstOrDefault(c => c.Type == "Id");
        var role = identityClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        if ((id != null && Convert.ToInt64(id.Value) == userId) || role is { Value: "Admin" })
        {
            var subscriptions = _repository.GetThemeSubscriptionsForUser(userId);
            var subscribersRead = _mapper.Map<IEnumerable<ThemeSubscriptionReadDto>>(subscriptions);
            return Ok(subscribersRead);
        }
        
        return Empty;
    }
    
    [HttpGet("Theme={themeId}")]
    [Authorize(Policy = "Admin")]
    public ActionResult<IEnumerable<ThemeSubscriptionReadDto>> GetSubscriptionsForTheme(long themeId)
    {
        var subscriptions = _repository.GetSubscriptionsByTheme(themeId);
        var subscribersRead = _mapper.Map<IEnumerable<ThemeSubscriptionReadDto>>(subscriptions);
        return Ok(subscribersRead);
    }

    private IEnumerable<Claim> GetUserClaims()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var identityClaims = identity.Claims;
        return identityClaims;
    }

    private long GetUserId()
    {
        var claims = GetUserClaims();
        var idClaim = claims.FirstOrDefault(c => c.Type == "Id");
        var id = Convert.ToInt64(idClaim.Value);
        return id;
    }
}