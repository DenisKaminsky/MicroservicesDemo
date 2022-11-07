using Microservices.RabbitMQ.Interfaces;
using Microservices.RabbitMQ.Services;
using Microservices.RabbitMQ.Services.Platform;
using Microservices.RabbitMQ.Types;
using Microservices.RabbitMQ.Types.Platform;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Microservices.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{
    #region Producers
    public static IServiceCollection AddPlatformMessageBusProducer(this IServiceCollection services, MessageBusOptions options)
    {
        return services
            .AddSingleton(
                typeof(IMessageBusProducer<PlatformEventBase>),
                x => new MessageBusProducer<PlatformEventBase>(
                    x.GetRequiredService<ConnectionFactory>(),
                    options.ExchangeName,
                    options.QueueName,
                    options.QueueAndExchangeRoutingKey));
    }

    #endregion

    #region Consumers
    public static IServiceCollection AddPlatformMessageBusConsumer(this IServiceCollection services, MessageBusOptions options)
    {
        return services.AddHostedService(x =>
            new PlatformMessageBusConsumer(
                x.GetRequiredService<IServiceScopeFactory>(),
                x.GetRequiredService<ConnectionFactory>(),
                options.ExchangeName,
                options.QueueName,
                options.QueueAndExchangeRoutingKey
            ));
    }

    #endregion

    public static IServiceCollection AddConnectionFactory(this IServiceCollection services, MessageBusServerOptions options)
    {
        return services.AddSingleton(_ => new ConnectionFactory
        {
            HostName = options.Host,
            Port = options.Port,
            DispatchConsumersAsync = true
        });
    }
}