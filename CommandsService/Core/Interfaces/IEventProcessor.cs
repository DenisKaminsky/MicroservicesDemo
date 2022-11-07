namespace CommandsService.Core.Interfaces;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}