using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Models;

namespace PlatformService.Data.Seed
{
    public static class PlatformSeed
    {
        public static void SeedPlatforms(IApplicationBuilder applicationBuilder, bool isProd)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            Seed(serviceScope.ServiceProvider.GetService<AppDbContext>()!, isProd);
        }

        private static void Seed(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations");
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
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(new[]
                {
                    new Platform
                    {
                        Name = "Dot Net",
                        Cost = "Free",
                        Publisher = "Microsoft"
                    },
                    new Platform
                    {
                        Name = "SQL Server Express",
                        Cost = "Free",
                        Publisher = "Microsoft"
                    },
                    new Platform
                    {
                        Name = "Kubernetes",
                        Cost = "Free",
                        Publisher = "Cloud Native Computing Foundation"
                    }
                });

                context.SaveChanges();

                Console.WriteLine("--> Done");
            }
            else
            {
                Console.WriteLine("--> We already have data in our DB");
            }
        }
    }
}