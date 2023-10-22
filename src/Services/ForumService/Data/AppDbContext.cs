using Microsoft.EntityFrameworkCore;
using ForumService.Models;

namespace ForumService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Theme>().HasOne(e => e.ParentTheme).WithMany(e => e.ChildThemes).HasForeignKey(e => e.ParentThemeId);
        modelBuilder.Entity<Post>().HasOne(e => e.Theme).WithMany(e => e.Posts).HasForeignKey(e => e.ThemeId);
        modelBuilder.Entity<Comment>().HasOne(e => e.Post).WithMany(e => e.Comments).HasForeignKey(e => e.PostId);
        modelBuilder.Entity<Comment>().HasMany(e => e.Comments).WithOne(e => e.ParentComment).HasForeignKey(e => e.ParentCommentId).IsRequired(false);
    }
}