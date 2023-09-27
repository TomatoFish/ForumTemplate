using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PostService.Dtos;

public class PostCreateDto
{
    public long UserId { get; set; }
    
    public long ThemeId { get; set; }
    
    public string? Content { get; set; }
}