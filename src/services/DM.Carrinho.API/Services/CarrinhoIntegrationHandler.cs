using DM.Carrinho.API.Data;
using DM.Core.Messages.Integration;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DM.Carrinho.API.Services;

public class CarrinhoIntegrationHandler : IConsumer<PedidoRealizadoIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public CarrinhoIntegrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<PedidoRealizadoIntegrationEvent> context)
    {
        await RemoveShoppingCart(context.Message);
        await context.RespondAsync(new PedidoRealizadoResponse());
    }


    private async Task RemoveShoppingCart(PedidoRealizadoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

        var shoppingCart = await context.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

        if (shoppingCart != null)
        {
            context.CarrinhoCliente.Remove(shoppingCart);
            await context.SaveChangesAsync();
        }
    }
}
public record PedidoRealizadoResponse;
