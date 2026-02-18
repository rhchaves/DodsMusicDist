using DM.MessageBus;
using System.Reflection;

namespace DM.Clientes.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration, Assembly.GetAssembly(typeof(Program)));
    }
}