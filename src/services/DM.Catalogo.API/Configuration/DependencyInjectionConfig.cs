using DM.Catalogo.API.Data;
using DM.Catalogo.API.Data.Repository;
using DM.Catalogo.API.Models;

namespace DM.Catalogo.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<CatalogoContext>();
    }
}
