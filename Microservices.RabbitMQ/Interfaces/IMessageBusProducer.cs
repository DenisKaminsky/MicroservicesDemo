using Microservices.RabbitMQ.Types;

namespace Microservices.RabbitMQ.Interfaces;

public interface IMessageBusProducer<in T> 
    where T : EventBase
{
    void Publish<TP>(TP @event) where TP : T;
}