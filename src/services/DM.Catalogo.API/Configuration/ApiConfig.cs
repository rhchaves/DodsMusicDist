using DM.Catalogo.API.Data;
using DM.WebAPI.Core.Identidade;
using Microsoft.EntityFrameworkCore;

namespace DM.Catalogo.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
