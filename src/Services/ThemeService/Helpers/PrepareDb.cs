using ThemeService.Data;
using ThemeService.Models;
using Microsoft.EntityFrameworkCore;

namespace ThemeService.Helpers;

public static class PrepareDb
{
    public static void PreparePopulation(IApplicationBuilder app, bool isProduction)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            AppDbContext? context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            if (context != null)
            {
                SeedData(context, isProduction);
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

        if (!dbContext.Themes.Any())
        {
            Console.WriteLine("--> Seeding data...");

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
            Console.WriteLine("--> Database already populated");
        }
    }
}