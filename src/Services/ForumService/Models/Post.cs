using System.ComponentModel.DataAnnotations;

namespace ForumService.Models;

public class Post
{
    [Required]
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public long? ThemeId { get; set; }
    
    [Required]
    public Theme? Theme { get; set; }
    
    [Required]
    public string? Header { get; set; }
    
    [Required]
    public string? Content { get; set; }

    [Required]
    public DateTime CreationTimeStamp { get; set; }
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}