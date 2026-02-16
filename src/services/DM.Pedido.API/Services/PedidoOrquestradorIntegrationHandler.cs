using DM.Core.Messages.Integration;
using DM.Pedidos.API.Application.Queries;
using MassTransit;

namespace DM.Pedidos.API.Services;

public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
{
    private readonly ILogger<PedidoOrquestradorIntegrationHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public PedidoOrquestradorIntegrationHandler(ILogger<PedidoOrquestradorIntegrationHandler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedido inicializado.");

        _timer = new Timer(OrquestradorPedidos, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedido finalizado.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async void OrquestradorPedidos(object state)
    {
        using var escopo = _serviceProvider.CreateScope();

        var pedidoRequisicao = escopo.ServiceProvider.GetRequiredService<IPedidoQueries>();
        var pedido = await pedidoRequisicao.ObterPedidoAutorizado();

        if (pedido == null) return;

        var bus = escopo.ServiceProvider.GetRequiredService<IBus>();

        var authorizedOrder = new PedidoAutorizadoIntegrationEvent(pedido.ClienteId, pedido.Id,
            pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

        await bus.Publish(authorizedOrder);

        _logger.LogInformation($"O pedido ID: {pedido.Id} foi enviado para reduzir o estoque.");
    }
}