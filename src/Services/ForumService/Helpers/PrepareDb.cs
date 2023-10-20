using ForumService.Data;
using ForumService.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumService.Helpers;

public static class PrepareDb
{
    public static void PreparePopulation(IApplicationBuilder app, bool isProduction)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            AppDbContext? dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            if (dbContext != null)
            {
                SeedData(dbContext, isProduction);
            }
        }
    }

    private static void SeedData(AppDbContext dbContext, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Applying migrations...");
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Can't run migrations: {ex.Message}");
            }

            return;
        }

        SeedThemeData(dbContext);
        SeedPostData(dbContext);
        SeedCommentData(dbContext);
    }

    private static void SeedThemeData(AppDbContext dbContext)
    {
        if (!dbContext.Themes.Any())
        {
            Console.WriteLine("--> Seeding theme data...");

            var firstItem = new Theme()
            {
                UserId = 1, Name = "Theme 1",
                CreationTimeStamp = new DateTime(2023, 9, 11, 13, 25, 33)
            };
            
            dbContext.Themes.AddRange(new List<Theme>()
            {
                firstItem,
                new Theme()
                {
                    UserId = 1, Name = "Theme 2",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 15, 3, 1)
                }
            });
            dbContext.Themes.Add(new Theme()
            {
                ParentTheme = firstItem,  UserId = 2, Name = "Theme 3", Description = "Included in theme 1",
                CreationTimeStamp = new DateTime(2023, 9, 12, 16, 48, 15)
            });
            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Database for themes already populated");
        }
    }
    
    private static void SeedPostData(AppDbContext dbContext)
    {
        if (!dbContext.Posts.Any())
        {
            Console.WriteLine("--> Seeding post data...");

            dbContext.Posts.AddRange(new List<Post>()
            {
                new Post()
                {
                    UserId = 1, Theme = dbContext.Themes.First(t => t.Name == "Theme 1"), Header = "Header 1", Content = "Post 1 from user 1 in theme 1",
                    CreationTimeStamp = new DateTime(2023, 9, 11, 13, 25, 33)
                },
                new Post()
                {
                    UserId = 2, Theme = dbContext.Themes.First(t => t.Name == "Theme 2"), Header = "Header 2", Content = "Post 1 from user 2 in theme 2",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 15, 3, 1)
                },
                new Post()
                {
                    UserId = 1, Theme = dbContext.Themes.First(t => t.Name == "Theme 3"), Header = "Header 3", Content = "Post 2 from user 1 in theme 3",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 16, 48, 15)
                }
            });
            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Database for posts already populated");
        }
    }

    private static void SeedCommentData(AppDbContext dbContext)
    {
        if (!dbContext.Comments.Any())
        {
            Console.WriteLine("--> Seeding comment data...");

            var firstComment = new Comment()
            {
                UserId = 2, Post = dbContext.Posts.First(p => p.Header == "Header 1"),
                Content = "Comment 1 from user 2 in post 1",
                CreationTimeStamp = new DateTime(2023, 9, 11, 14, 25, 33)
            };
            dbContext.Comments.AddRange(new List<Comment>()
            {
                firstComment,
                new Comment()
                {
                    UserId = 1, Post = dbContext.Posts.First(p => p.Header == "Header 2"),
                    Content = "Comment 1 from user 1 in post 2",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 16, 3, 1)
                },
                new Comment()
                {
                    UserId = 2, Post = dbContext.Posts.First(p => p.Header == "Header 3"),
                    Content = "Comment 1 from user 2 in post 3",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 17, 48, 15)
                }
            });
            dbContext.Comments.Add(new Comment()
            {
                UserId = 1, Post = dbContext.Posts.First(p => p.Header == "Header 1"), ParentComment = firstComment,
                Content = "Comment 2 from user 1 in post 1",
                CreationTimeStamp = new DateTime(2023, 9, 11, 14, 35, 5)
            });
            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Database for comments already populated");
        }
    }
}