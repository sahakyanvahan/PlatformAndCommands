using PlatformService.Models;

namespace PlatformService.Data;

public static class PlatformSeed
{
    public static void Seed(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        SeedData(scope.ServiceProvider.GetService<ApplicationDbContext>());
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
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