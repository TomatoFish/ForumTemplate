using ForumService.Models;

namespace ForumService.Data;

public interface IThemeRepo
{
    void SaveChanges();
   
    void CreateTheme(Theme post);

    IEnumerable<Theme> GetAllThemes();
    
    IEnumerable<Theme> GetThemesWithFilters(int? parentThemeId = null, int? userId = null, int? themeId = null);
}