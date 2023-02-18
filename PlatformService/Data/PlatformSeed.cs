using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PlatformSeed
{
    public static void Seed(IApplicationBuilder app, bool isProduction)
    {
        using var scope = app.ApplicationServices.CreateScope();
        SeedData(scope.ServiceProvider.GetService<ApplicationDbContext>(), isProduction);
    }
    
    private static void SeedData(ApplicationDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }
        
        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data...");
        
            context.Platforms.AddRange(
                new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );
        
            context.SaveChanges();
        
            Console.WriteLine("--> Seeding data... Done!");
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}