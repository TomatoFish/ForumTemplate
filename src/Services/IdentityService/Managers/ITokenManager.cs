using IdentityService.Models;

namespace IdentityService.Managers;

public interface ITokenManager
{
    string CreateToken(User user);
}