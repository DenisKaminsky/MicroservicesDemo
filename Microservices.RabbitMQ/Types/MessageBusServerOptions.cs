namespace Microservices.RabbitMQ.Types;

public class MessageBusServerOptions
{
    public string Host { get; set; } = null!;

    public int Port { get; set; }
}