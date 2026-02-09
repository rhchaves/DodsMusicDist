using DM.Clientes.API.Application.Commands;
using DM.Core.Messages.Integration;
using MassTransit;
using MediatR;

namespace DM.Clientes.API.Services;

public class RegistroClienteIntegrationHandler : IConsumer<UsuarioRegistradoIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<UsuarioRegistradoIntegrationEvent> context)
    {
        await context.RespondAsync(await RegistrarCliente(context.Message));
        while (!context.CancellationToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromSeconds(15), context.CancellationToken);
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var sucesso = await mediator.Send(clienteCommand);

        return new ResponseMessage(sucesso);
    }
}
