using Microsoft.OpenApi;

namespace DM.Identidade.API.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "DodsMusic Identity API",
                Version = "v1",
                Description = "Esta API faz parte da plataforma DodsMusic para administrar a autenticação e autorização dos usuários.",
                Contact = new OpenApiContact() { Name = "Suporte", Email = "suporte@dodsmusic.com.br" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });

        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}