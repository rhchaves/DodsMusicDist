using Microsoft.AspNetCore.Authentication.Cookies;

namespace DM.Loja.MVC.Configuration;

public static class IdentidadeConfig
{
    public static void AddIdentidadeConfig(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/erro/403";
            });
    }

    public static void UseIdentidadeConfig(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
