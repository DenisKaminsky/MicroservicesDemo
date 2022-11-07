using CommandsService.Data.Interfaces;
using CommandsService.Data.Models;

namespace CommandsService.Data.Repositories;

public class PlatformRepository: IPlatformRepository
{
    private readonly AppDbContext _appDbContext;

    public PlatformRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _appDbContext.Platforms.ToArray();
    }

    public void CreatePlatform(Platform platform)
    {
        _appDbContext.Add(platform);
        _appDbContext.SaveChanges();
    }

    public bool PlatformExist(int platformId)
    {
        return _appDbContext.Platforms.Any(x => x.Id == platformId);
    }

    public bool ExternalPlatformExist(int externalPlatformId)
    {
        return _appDbContext.Platforms.Any(x => x.ExternalId == externalPlatformId);
    }
}