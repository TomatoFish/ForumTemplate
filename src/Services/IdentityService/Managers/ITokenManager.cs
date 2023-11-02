using IdentityService.Models;

namespace IdentityService.Managers;

public interface ITokenManager
{
    string CreateAccessToken(User user, DateTime expirationTime);
    string CreateRefreshToken();
}