using System.ComponentModel.DataAnnotations;

namespace ThemeService.Models;

public class Theme
{
    [Required]
    [Key]
    public long Id { get; set; }
    
    public Theme? ParentTheme { get; set; }
    
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    public string? Description { get; set; }

    [Required]
    public DateTime CreationTimeStamp { get; set; }
}