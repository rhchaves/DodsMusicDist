using DM.Core.Messages.Integration;
using EasyNetQ;

namespace DM.MessageBus;

public sealed class MessageBus : IMessageBus
{
    private readonly IBus _bus;

    public MessageBus(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : IntegrationEvent
    {
        return _bus.PubSub.PublishAsync(@event);
    }

    public Task SubscribeAsync<TEvent>(string subscriptionId, Func<TEvent, Task> handler) where TEvent : IntegrationEvent
    {
        return _bus.PubSub.SubscribeAsync(subscriptionId, handler);
    }

    public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        return _bus.Rpc.RequestAsync<TRequest, TResponse>(request);
    }

    public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
            where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        return _bus.Rpc.RespondAsync(responder);
    }
}
