using IdentityService.Data;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Helpers;

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

        if (!dbContext.Users.Any())
        {
            Console.WriteLine("--> Seeding data...");

            dbContext.Users.Add(
                new User()
                {
                    UserName = "user1", Email = "user1@email",
                    PasswordHash = "MTBWdzbYNzJnk6stAQ4v1CTpkZ7WwN2xwNe+M1aXhCRmH5Bk26NwtVbYR6vyWdB0RHmPuJZk6Iv5Q4/YnFj5829KwZc4VTvgoifYhm9fAIOldujRidC0kutYrvuCyVtCJCwVupRcmp27SNZbIcKWkW7+WEMsRXAgC7774WJlRsgUAu7W+WngTo8xcn6VOjCjgP7Hs+HAUfMunSE7n9G09RappXhp0U6PijeHZty9CkY3xz43vH5D3v86DzXRxaIotPWuUo1rfJd1fQyAEse/Cgc11hTofFBPYEb9TDO2IYyEtDvqCfOX67cxQEjfFq8ccAGvwwfdULBdjrgYC/WZnA==:/w3FCYw+Stl3RMfbBdUQ2g==",
                    FirstName = "f", LastName = "l",
                    DisplayRealName = false, Role = "User", RegistrationTimeStamp = DateTime.Now, IsActive = true
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