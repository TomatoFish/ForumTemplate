using Microsoft.EntityFrameworkCore;
using ThemeSubscriptionService.Models;

namespace ThemeSubscriptionService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<ThemeSubscription> ThemeSubscriptions { get; set; }
}