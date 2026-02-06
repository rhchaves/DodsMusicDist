using DM.Core.Utils;
using DM.MessageBus;

public static class MessageBusConfig
{
    public static void AddMessageBusConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
    }
}
