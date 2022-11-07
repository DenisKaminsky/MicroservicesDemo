using PlatformService.Data.Interfaces;
using PlatformService.Models;

namespace PlatformService.Data.Repositories;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _appDbContext;

    public PlatformRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<Platform> GetAll()
    {
        return _appDbContext.Platforms.ToArray();
    }

    public Platform? GetById(int id)
    {
        return _appDbContext.Platforms.FirstOrDefault(x => x.Id == id);
    }

    public void Create(Platform? platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _appDbContext.Add(platform);
    }

    public bool SaveChanges()
    {
        return _appDbContext.SaveChanges() >= 0;
    }
}