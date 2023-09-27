namespace ThemeService.Dtos;

public class ThemeReadDto
{
    public long Id { get; set; }
    
    public long? ParentThemeId { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }

    public DateTime CreationTimeStamp { get; set; }
}