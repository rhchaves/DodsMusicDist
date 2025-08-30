using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Services;

namespace DM.Loja.MVC.Configuration;

public static class InjecaoDependenciaConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddHttpClient<IAutenticacaoServico, AutenticacaoServico>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, AspNetUser>();
    }
}
