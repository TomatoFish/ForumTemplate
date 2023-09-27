namespace ThemeService.Dtos;

public class ThemeCreateDto
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int? ParentThemeId { get; set; }
}