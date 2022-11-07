using CommandsService.Data.Models;

namespace CommandsService.Core.Interfaces.Grpc;

public interface IPlatformDataClient
{
    IEnumerable<Platform> GetAllPlatforms();
}