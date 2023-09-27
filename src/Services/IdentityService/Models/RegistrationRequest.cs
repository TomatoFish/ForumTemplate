namespace IdentityService.Dtos;

public class RegistrationRequest
{
    public required string Username { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool DisplayRealName { get; set; }
    
    public required string Password { get; set; }
    
    public required string Email { get; set; }
    
    public string? Role { get; set; }
}