using DM.Catalogo.API.Models;
using DM.Core.DomainObjects;
using DM.Core.Messages.Integration;
using MassTransit;

namespace DM.Catalogo.API.Services;

public class CatalogoIntegrationHandler : IConsumer<PedidoAutorizadoIntegrationEvent>
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public CatalogoIntegrationHandler(IServiceProvider serviceProvider, IBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }
    
    public async Task Consume(ConsumeContext<PedidoAutorizadoIntegrationEvent> context)
    {
        await BaixarEstoque(context.Message);
    }

    private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();

        var produtosComEstoque = new List<Produto>();
        var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();

        var idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));
        var produtos = await produtoRepository.ObterProdutosPorId(idsProdutos);

        if (produtos.Count != message.Itens.Count)
        {
            await CancelarPedidoSemEstoque(message);
            return;
        }

        foreach (var produto in produtos)
        {
            var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;

            if (produto.EstaDisponivel(quantidadeProduto))
                continue;
         
            produto.RetirarEstoque(quantidadeProduto);
            produtosComEstoque.Add(produto);
        }

        if (produtosComEstoque.Count != message.Itens.Count)
        {
            await CancelarPedidoSemEstoque(message);
            return;
        }

        foreach (var produto in produtosComEstoque) produtoRepository.Atualizar(produto);

        if (!await produtoRepository.UnitOfWork.Commit())
            throw new DomainException($"Problemas ao atualizar estoque do pedido {message.PedidoId}");

        var pedidoBaixado = new PedidoBaixadoEstoqueIntegrationEvent(message.ClienteId, message.PedidoId);
        await _bus.Publish(pedidoBaixado);
    }

    public async Task CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.ClienteId, message.PedidoId);
        await _bus.Publish(pedidoCancelado);
    }
}