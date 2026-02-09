using DM.Core.Messages.Integration;
using MassTransit;
using MediatR;

namespace DM.Pedidos.API.Application.Events;

public class PedidoEventHandler : INotificationHandler<PedidoRealizadoEvent>
{
    private readonly IBus _bus;

    public PedidoEventHandler(IBus bus)
    {
        _bus = bus;
    }

    public async Task Handle(PedidoRealizadoEvent message, CancellationToken cancellationToken)
    {
        await _bus.Publish(new PedidoRealizadoIntegrationEvent(message.ClienteId), cancellationToken);
    }
}
