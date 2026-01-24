using DM.Carrinho.API.Data;
using DM.Loja.MVC.Extensions;

namespace DM.Catalogo.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, Usuario>();
        services.AddScoped<CarrinhoContext>();
    }
}
