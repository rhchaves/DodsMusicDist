using DM.Core.DomainObjects;
using DM.Core.Messages.Integration;
using DM.MessageBus;
using DM.Pagamentos.API.Models;
using MassTransit;

namespace DM.Pagamentos.API.Services;

public class PagamentoIntegrationHandler : IConsumer<PedidoIniciadoIntegrationEvent>, IConsumer<PedidoCanceladoIntegrationEvent>,
    IConsumer<PedidoBaixadoEstoqueIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBus _bus;

    public PagamentoIntegrationHandler(IServiceProvider serviceProvider, IBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    //private void SetResponder()
    //{
    //    _bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
    //        await AutorizarPagamento(request));
    //}

    //private void SetSubscribers()
    //{
    //    _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request =>
    //    await CancelarPagamento(request));

    //    _bus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque", async request =>
    //    await CapturarPagamento(request));
    //}

    //protected override Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    SetResponder();
    //    SetSubscribers();
    //    return Task.CompletedTask;
    //}


    public async Task Consume(ConsumeContext<PedidoIniciadoIntegrationEvent> context)
    {
        await context.RespondAsync(await AutorizarPagamento(context.Message));
    }

    public async Task Consume(ConsumeContext<PedidoCanceladoIntegrationEvent> context)
    {
        await context.RespondAsync(CancelarPagamento(context.Message));
    }

    public async Task Consume(ConsumeContext<PedidoBaixadoEstoqueIntegrationEvent> context)
    {
        await context.RespondAsync(CapturarPagamento(context.Message));
    }

    private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();
        var pagamento = new Pagamento
        {
            PedidoId = message.PedidoId,
            TipoPagamento = (TipoPagamento)message.TipoPagamento,
            Valor = message.Valor,
            CartaoCredito = new CartaoCredito(
                message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
        };

        return await pagamentoService.AutorizarPagamento(pagamento);
    }

    private async Task CancelarPagamento(PedidoCanceladoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

        var response = await pagamentoService.CancelarPagamento(message.PedidoId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
    }

    private async Task CapturarPagamento(PedidoBaixadoEstoqueIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

        var response = await pagamentoService.CapturarPagamento(message.PedidoId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");

        await _bus.Publish(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));
    }
}
