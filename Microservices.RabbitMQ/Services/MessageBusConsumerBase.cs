using System.Text;
using System.Text.Json;
using MediatR;
using Microservices.RabbitMQ.Enums;
using Microservices.RabbitMQ.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservices.RabbitMQ.Services;

internal abstract class MessageBusConsumerBase<T> : MessageBusClientBase, IHostedService
    where T : EventBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    protected MessageBusConsumerBase(
        IServiceScopeFactory serviceScopeFactory,
        ConnectionFactory connectionFactory, 
        string? exchangeName, 
        string? queueName, 
        string? queueAndExchangeRoutingKey) 
        : base(connectionFactory, exchangeName, queueName, queueAndExchangeRoutingKey)
    {
        _serviceScopeFactory = serviceScopeFactory;

        try
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += OnEventReceived;
            Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            Console.WriteLine($"--> Listening {queueName} queue");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Cannot listen {queueName} queue: {ex}");
        }
    }

    protected abstract T GetCommandFromEvent(EventType eventType, string eventBody);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }
    
    private async Task OnEventReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var body = Encoding.UTF8.GetString(@event.Body.ToArray());
            var eventTypeString = JsonSerializer.Deserialize<EventBase>(body)?.EventType;
            var eventType = Enum.TryParse<EventType>(eventTypeString, out var parsedType)
                ? parsedType
                : EventType.Undetermined;
            
            var command = GetCommandFromEvent(eventType, body);

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Error while retrieving message from queue: {ex}");
        }
        finally
        {
            Channel?.BasicAck(@event.DeliveryTag, false);
        }
    }
}