using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ForumService.Data;
using ForumService.Dtos;
using ForumService.Models;

namespace ForumService.Controllers;

[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IThemeRepo _repository;

    public ThemeController(IMapper mapper, IThemeRepo repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    [HttpGet(Name = "GetThemes")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<Theme>> GetThemes([FromQuery]int? parentThemeId = null, [FromQuery]int? userId = null, [FromQuery]int? themeId = null)
    {
        var themeModels = _repository.GetThemesWithFilters(parentThemeId, userId, themeId);
        if (!themeModels.Any())
        {
            return Empty;
        }

        var themeReads = _mapper.Map<IEnumerable<ThemeReadDto>>(themeModels);
        return Ok(themeReads);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult<Theme> CreateTheme(ThemeCreateDto themeCreate)
    {
        var themeModel = _mapper.Map<Theme>(themeCreate);
        themeModel.CreationTimeStamp = DateTime.UtcNow;
        themeModel.UserId = GetUserId();
        if (themeCreate.ParentThemeId != null)
        {
            var parentTheme = _repository.GetThemesWithFilters(null, null, themeCreate.ParentThemeId.Value).FirstOrDefault();
            if (parentTheme != null)
            {
                themeModel.ParentTheme = parentTheme;
            }
        }
        
        _repository.CreateTheme(themeModel);
        _repository.SaveChanges();
        
        var themeRead = _mapper.Map<ThemeReadDto>(themeModel);
        
        return CreatedAtRoute(nameof(GetThemes), new { themeRead.Id }, themeRead);
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