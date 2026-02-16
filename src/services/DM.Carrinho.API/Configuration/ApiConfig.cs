using DM.Carrinho.API.Data;
using DM.WebAPI.Core.Configuration;
using DM.WebAPI.Core.Identidade;
using Microsoft.EntityFrameworkCore;

namespace DM.Carrinho.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiCoreConfig(configuration).UseDbContext<CarrinhoContext>(configuration);
    }

    public static void UseApiConfig(this WebApplication app, IWebHostEnvironment env)
    {
        app.UseApiCoreConfig(env);
        app.UseAutenticacaoConfig();
        app.MapControllers();
    }
}
