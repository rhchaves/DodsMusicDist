using DM.Core.Messages.Integration;

namespace DM.MessageBus;

public interface IMessageBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
    Task SubscribeAsync<TEvent>(string subscriptionId, Func<TEvent, Task> handler) where TEvent : IntegrationEvent;
    Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : IntegrationEvent where TResponse : ResponseMessage;
    IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage;
}
