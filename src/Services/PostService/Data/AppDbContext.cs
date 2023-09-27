using Microsoft.EntityFrameworkCore;
using PostService.Models;

namespace PostService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Post> Posts { get; set; }
}