using CommandsService.Core.Interfaces.Grpc;
using CommandsService.Data.Interfaces;
using CommandsService.Data.Models;

namespace CommandsService.Data.Seed;

public static class PlatformSeed
{
    public static void SeedPlatforms(IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();

        var grpcClient = scope.ServiceProvider.GetService<IPlatformDataClient>();

        var platforms = grpcClient!.GetAllPlatforms();

        SeedData(scope.ServiceProvider.GetService<IPlatformRepository>()!, platforms);
    }

    private static void SeedData(IPlatformRepository platformRepository, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> seeding Platforms ...");
        foreach (var platform in platforms)
        {
            if (!platformRepository.ExternalPlatformExist(platform.ExternalId))
            {
                platformRepository.CreatePlatform(platform);
            }
        }
    }
}