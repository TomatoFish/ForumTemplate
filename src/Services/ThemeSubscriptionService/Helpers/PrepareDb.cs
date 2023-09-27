using ThemeSubscriptionService.Data;
using ThemeSubscriptionService.Models;
using Microsoft.EntityFrameworkCore;

namespace ThemeSubscriptionService.Helpers;

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

        if (!dbContext.ThemeSubscriptions.Any())
        {
            Console.WriteLine("--> Seeding data...");

            dbContext.ThemeSubscriptions.Add(
                new ThemeSubscription()
                {
                    UserId = 1, ThemeId = 1
                }
            );

            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Database already populated");
        }
    }
}