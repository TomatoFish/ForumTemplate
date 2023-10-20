namespace ForumService.Dtos;

public class PostCreateDto
{
    public long UserId { get; set; }
    
    public long ThemeId { get; set; }
    
    public string? Header { get; set; }
    
    public string? Content { get; set; }
}