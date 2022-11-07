using System.Text;
using System.Text.Json;
using Microservices.RabbitMQ.Interfaces;
using Microservices.RabbitMQ.Types;
using RabbitMQ.Client;

namespace Microservices.RabbitMQ.Services;

public sealed class MessageBusProducer<T> : MessageBusClientBase, IMessageBusProducer<T>
    where T : EventBase
{
    public MessageBusProducer(
        ConnectionFactory connectionFactory,
        string? exchangeName,
        string? queueName,
        string? queueAndExchangeRoutingKey
        ) : base(connectionFactory, exchangeName, queueName, queueAndExchangeRoutingKey)
    {
    }

    public void Publish<TP>(TP @event) 
        where TP : T
    {
        try
        {
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);
            var properties = Channel!.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 1;
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            Channel!.BasicPublish(
                exchange: ExchangeName,
                routingKey: QueueAndExchangeRoutingKey,
                basicProperties: properties,
                body: body);

            Console.WriteLine("--> Published successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Error while publishing: {ex}");
        }
    }
}