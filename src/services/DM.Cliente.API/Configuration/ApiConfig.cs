using DM.Clientes.API.Data;
using DM.WebAPI.Core.Configuration;

namespace DM.Clientes.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiCoreConfig(configuration).UseDbContext<ClientesContext>(configuration);
    }
}