namespace IdentityService.Models;

public class AuthenticationResponse
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime ExpirationTime { get; set; }
}