using DM.WebAPI.Core.Configuration;
using DM.WebAPI.Core.Identidade;
using DM.WebAPI.Core.Usuario;

namespace DM.Identidade.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddScoped<IUsuario, Usuario>();
        services.AddVerificacaoSaudeGenerica(configuration);

        return services;
    }

    public static IApplicationBuilder UseApiConfig(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        // Under certain scenarios, e.g. minikube / linux environment / behind load balancer
        // https redirection could lead dev's to overcomplicated configuration for testing purposes
        // In production is a good practice to keep it true
        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAutenticacaoConfig();
        app.UseJwksDiscovery();
        app.UseVerificacaoSaudeGenerica();

        return app;
    }
}
