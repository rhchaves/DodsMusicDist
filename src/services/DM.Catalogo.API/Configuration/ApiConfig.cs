using DM.Catalogo.API.Data;
using DM.WebAPI.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DM.Catalogo.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiCoreConfig(configuration).UseDbContext<CatalogoContext>(configuration);
    }
}
