namespace Microservices.RabbitMQ.Types.Platform;

public abstract class PlatformEventBase : EventBase
{
    public int Id { get; set; }
}