using Microsoft.EntityFrameworkCore;
using ThemeService.Models;

namespace ThemeService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Theme> Themes { get; set; }
}