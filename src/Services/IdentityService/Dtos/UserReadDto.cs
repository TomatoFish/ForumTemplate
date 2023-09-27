namespace IdentityService.Dtos;

public class UserReadDto
{
    public long Id { get; set; }
    
    public string? Username { get; set; }
    
    public string? Email { get; set; }
    
    public string? UserPic { get; set; }
    
    public string? Role { get; set; }
    
    public DateTime RegistrationTimeStamp { get; set; }

    public bool IsActive { get; set; } = true;
}