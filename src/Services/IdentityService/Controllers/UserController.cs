using AutoMapper;
using IdentityService.Data;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Dtos;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserRepo _repository;

    public UserController(IMapper mapper, IUserRepo repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet(Name = "GetAllUsers")]
    [Authorize(Policy = "Admin")]
    public ActionResult<IEnumerable<UserReadDto>> GetAllUsers()
    {
        var userModels = _repository.GetAllUsers();

        var userReads = new List<UserReadDto>();
        foreach (var user in userModels)
        {
            userReads.Add(CreateUserRead(user));
        }
        
        return Ok(userModels);
    }

    [HttpGet("id={id}", Name = "GetUserById")]
    public ActionResult<UserReadDto> GetUserById(long id)
    {
        var userModel = _repository.GetUserById(id);

        return GetUserReadActionResult(userModel);
    }
    
    [HttpGet("email={email}", Name = "GetUserByEmail")]
    public ActionResult<UserReadDto> GetUserByEmail(string email)
    {
        var userModel = _repository.GetUserByEmail(email);

        return GetUserReadActionResult(userModel);
    }
    
    [HttpGet("username={username}", Name = "GetUserByUsername")]
    public ActionResult<UserReadDto> GetUserByUsername(string username)
    {
        var userModel = _repository.GetUserByUsername(username);

        return GetUserReadActionResult(userModel);
    }


    private UserReadDto CreateUserRead(User? userModel)
    {
        if (userModel == null)
        {
            return null;
        }
        
        var userRead = _mapper.Map<UserReadDto>(userModel);
        if (userModel.DisplayRealName)
        {
            userRead.Username = $"{userModel.FirstName} {userModel.LastName}";
        }

        return userRead;
    }
    
    private ActionResult<UserReadDto> GetUserReadActionResult(User? userModel)
    {
        var userRead = CreateUserRead(userModel);
        if (userRead == null)
        {
            return Empty;
        }
        
        return Ok(userRead);
    }
}