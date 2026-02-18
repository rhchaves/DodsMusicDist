using DM.WebAPI.Core.DatabaseType;
using DM.WebAPI.Core.Identidade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static DM.WebAPI.Core.DatabaseType.ProvedorConfig;

namespace DM.WebAPI.Core.Configuration;

public static class ApiCoreConfig
{
    public static IServiceCollection AddApiCoreConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        return services;
    }

    public static IServiceCollection UseDbContext<TContext>(this IServiceCollection services,
        IConfiguration configuration) where TContext : DbContext
    {
        services.ConfigurarProvedorParaContexto<TContext>(DetectarBancoDeDados(configuration));

        return services;
    }

    public static void UseApiCoreConfig(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi();
            //app.UseReDoc(options => { options.Path = "/redoc"; });
        }

        // Under certain scenarios, e.g. minikube / linux environment / behind load balancer
        // https redirection could lead devs to overcomplicate configuration for testing purposes
        // In production is a good practice to keep it true
        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAutenticacaoConfig();

        app.MapControllers();

    }
}
