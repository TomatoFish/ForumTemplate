using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models;

public class User
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public required string UserName { get; set; }
    
    [Required]
    public required string Email { get; set; }
    
    [Required]
    public required string PasswordHash { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public bool DisplayRealName { get; set; } = false;
    
    public string? UserPic { get; set; }
    
    [Required]
    public string? Role { get; set; }
    
    [Required]
    public DateTime RegistrationTimeStamp { get; set; }

    public bool IsActive { get; set; } = true;
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpirationTime { get; set; }
}