using DM.Pagamentos.API.Data;
using DM.Pagamentos.API.Facade;
using DM.WebAPI.Core.Configuration;

namespace DM.Pagamentos.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiCoreConfig(configuration).UseDbContext<PagamentosContext>(configuration)
            .Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));
    }

}

