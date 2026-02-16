using DM.Bff.Compras.Extensions;
using DM.WebAPI.Core.Identidade;

namespace DM.Bff.Compras.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.Configure<AppServicesConfig>(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
    }

    public static void UseApiConfig(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors("Total");
        app.UseAutenticacaoConfig();
        app.MapControllers();
    }
}
