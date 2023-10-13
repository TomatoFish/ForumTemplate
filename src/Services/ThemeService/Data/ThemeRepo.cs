using ThemeService.Models;

namespace ThemeService.Data;

public class ThemeRepo : IThemeRepo
{
    private readonly AppDbContext _context;

    public ThemeRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreateTheme(Theme post)
    {
        _context.Add(post);
    }

    public IEnumerable<Theme> GetAllThemes()
    {
        return _context.Themes;
    }
    
    public IEnumerable<Theme> GetThemesWithFilters(int? parentThemeId = null, int? userId = null, int? themeId = null)
    {
        return _context.Themes.Where(post =>
            (parentThemeId == null || (parentThemeId.Value == 0 && post.ParentTheme == null) || (post.ParentTheme != null && post.ParentTheme.Id == parentThemeId.Value)) &&
            (userId == null || post.UserId == userId.Value) &&
            (themeId == null || post.Id == themeId.Value));
    }
}