using DM.Identidade.API.Services;
using DM.WebAPI.Core.Identidade;
using DM.WebAPI.Core.Usuario;
using NetDevPack.Security.JwtSigningCredentials.AspNetCore;

namespace DM.Identidade.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddScoped<AutenticacaoServico>();
        services.AddScoped<IUsuario, Usuario>();

        return services;
    }

    public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAutenticacaoConfig();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseJwksDiscovery();

        return app;
    }
}
