namespace ForumService.Dtos;

public class CommentReadDto
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public long? PostId { get; set; }
    
    public long? ParentCommentId { get; set; }
    
    public string? Content { get; set; }

    public DateTime CreationTimeStamp { get; set; }
}