using AutoMapper;
using ForumService.Helpers;
using IdentityService.Data;
using IdentityService.Dtos;
using IdentityService.Helpers;
using IdentityService.Managers;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserRepo _repository;
    private readonly ITokenManager _tokenManager;
    private readonly ILogger _logger;

    public AuthenticationController(IMapper mapper, IUserRepo repository, ITokenManager tokenManager, ILogger<AuthenticationController> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _tokenManager = tokenManager;
        _logger = logger;
    }

    [HttpPost("login", Name = "Login")]
    public ActionResult<AuthenticationResponse> Login(AuthenticationRequest request)
    {
        var user = _repository.GetUserByUsername(request.Login);
        if (user == null || !Crypto.Compare(request.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        var expirationTime = DateTime.UtcNow.AddHours(2); //todo: remove hardcode
        var token = _tokenManager.CreateAccessToken(user, expirationTime);
        var refreshToken = _tokenManager.CreateRefreshToken();
        var refreshTokenExpirationTime = DateTime.UtcNow.AddDays(7); //todo: remove hardcode
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpirationTime = refreshTokenExpirationTime;
        
        _repository.UpdateUser(user);
        _repository.SaveChanges();
        
        _logger.LogInformation("--> User {username} with id {id} logged in.", user.UserName, user.Id);
        
        var response = new AuthenticationResponse()
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpirationTime = expirationTime
        };
        
        return Ok(response);
    }

    [HttpPost("register")]
    public ActionResult<AuthenticationResponse> Registration(RegistrationRequest registrationRequest)
    {
        var usernameAvailable = _repository.GetUserByUsername(registrationRequest.Username) == null;
        var emailAvailable = _repository.GetUserByEmail(registrationRequest.Email) == null;
        var passwordValid = UserHelper.CheckPasswordRules(registrationRequest.Password);
        
        if (!usernameAvailable || !emailAvailable || !passwordValid)
        {
            return Empty;
        }

        var userModel = _mapper.Map<User>(registrationRequest);
        if (string.IsNullOrEmpty(userModel.FirstName) || string.IsNullOrEmpty(userModel.LastName))
        {
            userModel.DisplayRealName = false;
        }
        if (string.IsNullOrEmpty(userModel.Role))
        {
            userModel.Role = "user";
        }
        userModel.PasswordHash = Crypto.Encrypt(registrationRequest.Password);
        userModel.RegistrationTimeStamp = DateTime.UtcNow;
        userModel.IsActive = true;
        
        _repository.CreateUser(userModel);
        _repository.SaveChanges();
        
        _logger.LogInformation("--> User {username} registered.", userModel.UserName);
        
        var request = _mapper.Map<AuthenticationRequest>(registrationRequest);
        request.RememberLogin = false;
        
        return CreatedAtRoute(nameof(Login), new { request }, request);
    }
    
    [HttpPost("refreshtoken", Name = "RefreshToken")]
    public ActionResult<AuthenticationResponse> RefreshToken([FromBody]RefreshTokenRequest request)
    {
        var userId = UserHelper.GetUserId(HttpContext);
        var user = _repository.GetUserById(userId);
        
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpirationTime <= DateTime.UtcNow)
            return BadRequest("Invalid token refresh request");

        var expirationTime = DateTime.UtcNow.AddMinutes(120); //todo: remove hardcode
        var token = _tokenManager.CreateAccessToken(user, expirationTime);
        var refreshToken = _tokenManager.CreateRefreshToken();
        var refreshTokenExpirationTime = DateTime.UtcNow.AddDays(7); //todo: remove hardcode
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpirationTime = refreshTokenExpirationTime;
        
        _repository.UpdateUser(user);
        _repository.SaveChanges();
        
        _logger.LogInformation("--> Refresh token for user {username} with id {id}.", user.UserName, user.Id);
        
        var response = new AuthenticationResponse()
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpirationTime = expirationTime
        };
        
        return Ok(response);
    }
    
    [Authorize("User")]
    [HttpPost("revoketoken", Name = "RevokeToken")]
    public ActionResult RevokeToken()
    {
        var userId = UserHelper.GetUserId(HttpContext);
        var user = _repository.GetUserById(userId);
        
        if (user == null)
        {
            return Unauthorized();
        }

        user.RefreshToken = null;
        user.RefreshTokenExpirationTime = DateTime.UtcNow;
        
        _repository.UpdateUser(user);
        _repository.SaveChanges();
        
        _logger.LogInformation("--> Revoke token for user {username} with id {id}.", user.UserName, user.Id);
        
        return Ok();
    }
}