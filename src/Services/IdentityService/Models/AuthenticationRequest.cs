namespace IdentityService.Models;

public class AuthenticationRequest
{
    public required string Login { get; set; }

    public required string Password { get; set; }

    public required bool RememberLogin { get; set; }
}