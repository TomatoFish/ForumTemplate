using System.ComponentModel.DataAnnotations;

namespace ForumService.Models;

public class Comment
{
    [Required]
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public long? PostId { get; set; }
    
    [Required]
    public Post? Post { get; set; }
    
    public long? ParentCommentId { get; set; }
    
    public Comment? ParentComment { get; set; }
    
    [Required]
    public string? Content { get; set; }

    [Required]
    public DateTime CreationTimeStamp { get; set; }
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}