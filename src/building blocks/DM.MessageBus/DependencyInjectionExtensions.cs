using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace DM.MessageBus;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, string connection)
    {
        if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentNullException(nameof(connection));

        services.AddEasyNetQ(connection);
        services.AddSingleton<IMessageBus, MessageBus>();

        return services;
    }
}