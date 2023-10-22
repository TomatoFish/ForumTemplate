namespace ForumService.Dtos;

public class CommentCreateDto
{
    public long PostId { get; set; }
    
    public long? ParentCommentId { get; set; }
    
    public string? Content { get; set; }
}