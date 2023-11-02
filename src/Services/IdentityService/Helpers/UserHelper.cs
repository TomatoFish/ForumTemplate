using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ForumService.Helpers;

public static class UserHelper
{
    public static bool CheckPasswordRules(string password)
    {
        var passwordLengthOk = password.Length >= 8;
        var passwordNumberOk = Regex.Count(password, @"\d") > 0;

        return passwordLengthOk && passwordNumberOk;
    }
    
    public static long GetUserId(HttpContext context)
    {
        var claims = GetUserClaims(context);
        var idClaim = claims.FirstOrDefault(c => c.Type == "Id");
        var id = Convert.ToInt64(idClaim.Value);
        return id;
    }
    
    private static IEnumerable<Claim> GetUserClaims(HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        var identityClaims = identity.Claims;
        return identityClaims;
    }
}