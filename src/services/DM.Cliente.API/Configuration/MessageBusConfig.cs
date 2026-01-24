using DM.Clientes.API.Services;
using DM.Core.Utils;
using DM.MessageBus;

namespace DM.Clientes.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<RegistroClienteIntegrationHandler>();
    }
}