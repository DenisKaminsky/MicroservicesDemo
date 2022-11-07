using System.Text.Json;
using MediatR;
using Microservices.RabbitMQ.Enums;
using Microservices.RabbitMQ.Types.Platform;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Microservices.RabbitMQ.Services.Platform;

internal class PlatformMessageBusConsumer: MessageBusConsumerBase<PlatformEventBase>
{
    public PlatformMessageBusConsumer(
        IServiceScopeFactory serviceScopeFactory, 
        ConnectionFactory connectionFactory, 
        string? exchangeName, 
        string? queueName, 
        string? queueAndExchangeRoutingKey) 
        : base(serviceScopeFactory, connectionFactory, exchangeName, queueName, queueAndExchangeRoutingKey)
    {
    }

    protected override PlatformEventBase GetCommandFromEvent(EventType eventType, string eventBody)
    {
        return eventType switch
        {
            EventType.PlatformPublished => JsonSerializer.Deserialize<PlatformPublishedEvent>(eventBody)!,
            _ => throw new ArgumentException("Cannot determine EventType")
        };
    }
}