using System.ComponentModel.DataAnnotations;

namespace ForumService.Models;

public class Theme
{
    [Required]
    [Key]
    public long Id { get; set; }
    
    public long? ParentThemeId { get; set; }
    
    public Theme? ParentTheme { get; set; }
    
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    public string? Description { get; set; }

    [Required]
    public DateTime CreationTimeStamp { get; set; }

    public ICollection<Theme> ChildThemes { get; set; } = new List<Theme>();
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}