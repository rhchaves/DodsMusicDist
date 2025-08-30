using Microsoft.AspNetCore.Authentication.Cookies;

namespace DM.Loja.MVC.Configuration;

public static class IdentidadeConfig
{
    public static void AddIdentidadeConfiguracao(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/erro/403";
            });
    }

    public static void UseIdentidadeConfiguracao(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
