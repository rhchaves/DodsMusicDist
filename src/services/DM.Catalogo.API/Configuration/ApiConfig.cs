using DM.WebAPI.Core.Identidade;

namespace DM.Catalogo.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services)
    {
        services.AddControllers();

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
        app.UseCors("Total");
        app.UseAutenticacaoConfig();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
