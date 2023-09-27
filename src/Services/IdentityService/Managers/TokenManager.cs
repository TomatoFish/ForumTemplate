using System.Security.Claims;
using System.Text;
using IdentityService.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Managers;

public class TokenManager : ITokenManager
{
    private readonly IConfiguration _config;

    public TokenManager(IConfiguration config)
    {
        _config = config;
    }
    
    public string CreateToken(User user)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        var tokenHandler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
        {
            Issuer = _config["JWT:TokenIssuer"],
            Audience = _config["JWT:Audience"],
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            }),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        });
        
        return token;
    }
}