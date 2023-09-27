using System.Text.RegularExpressions;
using AutoMapper;
using IdentityService.Data;
using IdentityService.Dtos;
using IdentityService.Helpers;
using IdentityService.Managers;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Models;

namespace IdentityService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IUserRepo _repository;

    //private readonly UserManager<User> _userManager;
    private readonly ITokenManager _tokenManager;

    public AuthenticationController(IMapper mapper, IUserRepo repository, ITokenManager tokenManager)
    {
        _mapper = mapper;
        _repository = repository;
        //_userManager = userManager;
        _tokenManager = tokenManager;
    }

    [HttpPost("/login", Name = "Login")]
    public ActionResult<AuthenticationResponse> Login(AuthenticationRequest request)
    {
        //var user = _userManager.Users.FirstOrDefault(u => u.UserName == request.Login);
        var user = _repository.GetUserByUsername(request.Login);
        if (user == null || !Crypto.Compare(request.Password, user.PasswordHash))
        {
            return Unauthorized();
        }
        
        var token = _tokenManager.CreateToken(user);
        
        var response = new AuthenticationResponse()
        {
            Token = token
        };
        
        return Ok(response);
    }

    [HttpPost("/register")]
    public ActionResult<AuthenticationResponse> Registration(RegistrationRequest registrationRequest)
    {
        var usernameAvailable = _repository.GetUserByUsername(registrationRequest.Username) == null;
        var emailAvailable = _repository.GetUserByEmail(registrationRequest.Email) == null;
        var passwordValid = CheckPasswordRules(registrationRequest.Password);
        
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
        
        var request = _mapper.Map<AuthenticationRequest>(registrationRequest);
        request.RememberLogin = false;
        
        return CreatedAtRoute(nameof(Login), new { request }, request);
    }
    
    private bool CheckPasswordRules(string password)
    {
        var passwordLengthOk = password.Length >= 8;
        var passwordNumberOk = Regex.Count(password, @"\d") > 0;

        return passwordLengthOk && passwordNumberOk;
    }
}