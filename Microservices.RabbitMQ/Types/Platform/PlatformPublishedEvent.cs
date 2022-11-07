using MediatR;

namespace Microservices.RabbitMQ.Types.Platform;

public class PlatformPublishedEvent : PlatformEventBase, IRequest<Unit>
{
    public string Name { get; set; } = null!;
}