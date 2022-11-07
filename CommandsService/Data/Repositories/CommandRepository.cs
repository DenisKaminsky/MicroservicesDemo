using CommandsService.Data.Interfaces;
using CommandsService.Data.Models;

namespace CommandsService.Data.Repositories;

public class CommandRepository: ICommandRepository
{
    private readonly AppDbContext _appDbContext;

    public CommandRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public IEnumerable<Command> GetAllCommands()
    {
        return _appDbContext.Commands.ToArray();
    }

    public IEnumerable<Command> GetCommandsByPlatformId(int platformId)
    {
        return _appDbContext.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(x => x.Platform.Name)
            .ToArray();
    }

    public Command? GetCommandByPlatformId(int commandId, int platformId)
    {
        return _appDbContext.Commands
            .FirstOrDefault(x =>
                x.Id == commandId
                && x.PlatformId == platformId);
    }

    public void CreateCommand(Command command)
    {
        _appDbContext.Commands.Add(command);
        _appDbContext.SaveChanges();
    }
}