using DM.Pedidos.Infra.Data;
using DM.WebAPI.Core.Configuration;

namespace DM.Pedidos.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiCoreConfig(configuration).UseDbContext<PedidosContext>(configuration);
    }
}
