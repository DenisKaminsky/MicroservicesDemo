using CommandsService.Data.Models;

namespace CommandsService.Data.Interfaces;

public interface ICommandRepository
{
    IEnumerable<Command> GetAllCommands();

    IEnumerable<Command> GetCommandsByPlatformId(int platformId);

    Command? GetCommandByPlatformId(int commandId, int platformId);

    void CreateCommand(Command command);
}