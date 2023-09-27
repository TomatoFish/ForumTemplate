using PostService.Data;
using PostService.Models;
using Microsoft.EntityFrameworkCore;

namespace PostService.Helpers;

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

        if (!dbContext.Posts.Any())
        {
            Console.WriteLine("--> Seeding data...");

            dbContext.Posts.AddRange(new List<Post>()
            {
                new Post()
                {
                    UserId = 1, ThemeId = 1, Content = "Message 1 from user 1 in theme 1",
                    CreationTimeStamp = new DateTime(2023, 9, 11, 13, 25, 33)
                },
                new Post()
                {
                    UserId = 2, ThemeId = 2, Content = "Message 1 from user 2 in theme 2",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 15, 3, 1)
                },
                new Post()
                {
                    UserId = 1, ThemeId = 2, Content = "Message 2 from user 1 in theme 2",
                    CreationTimeStamp = new DateTime(2023, 9, 12, 16, 48, 15)
                }
            });

            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Database already populated");
        }
    }
}