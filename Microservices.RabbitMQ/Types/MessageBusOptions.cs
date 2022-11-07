namespace Microservices.RabbitMQ.Types;

public class MessageBusOptions
{
    public string ExchangeName { get; set; } = null!;

    public string QueueName { get; set; } = null!;

    public string QueueAndExchangeRoutingKey { get; set; } = null!;
}