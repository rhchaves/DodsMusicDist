using DM.Core.DomainObjects;
using DM.Core.Messages.Integration;
using DM.Pedidos.Domain.Pedidos;
using MassTransit;

namespace DM.Pedidos.API.Services;

public class PedidoIntegrationHandler : IConsumer<PedidoCanceladoIntegrationEvent>, IConsumer<PedidoPagoIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public PedidoIntegrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<PedidoCanceladoIntegrationEvent> contexto)
    {
        await contexto.RespondAsync(CancelarPedido(contexto.Message));
    }

    public async Task Consume(ConsumeContext<PedidoPagoIntegrationEvent> contexto)
    {
        await contexto.RespondAsync(FinalizarPedido(contexto.Message));
    }

    private async Task CancelarPedido(PedidoCanceladoIntegrationEvent message)
    {
        using var escopo = _serviceProvider.CreateScope();

        var pedidoRepositorio = escopo.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepositorio.ObterPorId(message.PedidoId);
        pedido.CancelarPedido();

        pedidoRepositorio.Atualizar(pedido);

        if (!await pedidoRepositorio.UnitOfWork.Commit())
            throw new DomainException($"Problemas ao cancelar o pedido {message.PedidoId}");
    }

    private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
    {
        using var escopo = _serviceProvider.CreateScope();

        var pedidoRepositorio = escopo.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepositorio.ObterPorId(message.PedidoId);
        pedido.FinalizarPedido();

        pedidoRepositorio.Atualizar(pedido);

        if (!await pedidoRepositorio.UnitOfWork.Commit())
            throw new DomainException($"Problemas ao finalizar o pedido {message.PedidoId}");
    }
}