namespace PostService.Dtos;

public class PostReadDto
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public long ThemeId { get; set; }
    
    public string? Header { get; set; }
    
    public string? Content { get; set; }

    public DateTime CreationTimeStamp { get; set; }
}