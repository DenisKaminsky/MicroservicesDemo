using RabbitMQ.Client;

namespace Microservices.RabbitMQ.Services;

public abstract class MessageBusClientBase: IDisposable
{
    private IConnection? _connection;
    private readonly ConnectionFactory _connectionFactory;

    protected IModel? Channel;

    protected readonly string ExchangeName;
    protected readonly string QueueName;
    protected readonly string QueueAndExchangeRoutingKey;

    protected MessageBusClientBase(
        ConnectionFactory connectionFactory,
        string? exchangeName,
        string? queueName,
        string? queueAndExchangeRoutingKey)
    {
        _connectionFactory = connectionFactory;
        ExchangeName = exchangeName ?? "Microservices.Exchange";
        QueueName = queueName ?? "Microservices.Queue";
        QueueAndExchangeRoutingKey = queueAndExchangeRoutingKey ?? "Microservices.RoutingKey";

        ConnectToMessageBus();
    }

    private void ConnectToMessageBus()
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            Channel = _connection.CreateModel();

            Channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                //Exchange will not be deleted if RabbitMQ restarts
                durable: true,
                //Makes the exchange automatically delete if all of its queues unbind
                autoDelete: false);

            Channel.QueueDeclare(
                queue: QueueName,
                //This is whether the queue will survive when RabbitMQ restarts.
                durable: false,
                //This tells the queue to be used by only one connection and automatically delete when that connection closes
                exclusive: false,
                //Means that the queue, given that it had at least one consumer, will be deleted when all of its consumers unsubcribe.
                autoDelete: false);

            Channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: QueueAndExchangeRoutingKey);

            _connection.ConnectionShutdown += MessageBusConnectionShutdown;

            Console.WriteLine("--> Connected to RabbitMQ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to RabbitMQ: {ex.Message}");
        }
    }

    private void MessageBusConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown!");
    }

    public void Dispose()
    {
        Channel?.Close();
        _connection?.Close();
    }
}