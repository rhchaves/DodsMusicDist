using DM.WebAPI.Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using NetDevPack.Security.JwtExtensions;
using System.Diagnostics;

namespace DM.WebAPI.Core.Identidade;

public static class JwtConfig
{
    public static void AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfigSection = configuration.GetSection("AppSettings");
        services.Configure<AppConfig>(appConfigSection);

        var jwkOptions = appConfigSection.Get<JwkOptions>();
        jwkOptions.KeepFor = TimeSpan.FromMinutes(15);
        if (Debugger.IsAttached)
            IdentityModelEventSource.ShowPII = true;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.BackchannelHttpHandler = HttpExtensions.ConfigureClientHandler();
            options.SaveToken = true;
            options.SetJwksOptions(jwkOptions);
        });

        services.AddAuthorization();
    }

    public static void UseAutenticacaoConfig(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
