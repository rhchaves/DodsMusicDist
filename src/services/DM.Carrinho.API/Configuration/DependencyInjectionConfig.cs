using DM.Carrinho.API.Data;
using DM.WebAPI.Core.Usuario;

namespace DM.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, Usuario>();
        services.AddScoped<CarrinhoContext>();
    }
}
