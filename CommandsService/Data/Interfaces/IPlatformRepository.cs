using CommandsService.Data.Models;

namespace CommandsService.Data.Interfaces;

public interface IPlatformRepository
{
    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform platform);

    bool PlatformExist(int platformId);

    bool ExternalPlatformExist(int externalPlatformId);
}