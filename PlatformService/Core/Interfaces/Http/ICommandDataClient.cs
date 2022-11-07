using PlatformService.DTOs;

namespace PlatformService.Core.Interfaces.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommandAsync(PlatformReadDto platform);
}