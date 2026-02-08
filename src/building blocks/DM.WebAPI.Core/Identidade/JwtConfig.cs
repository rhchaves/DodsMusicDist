using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.JwtExtensions;

namespace DM.WebAPI.Core.Identidade;

public static class JwtConfig
{
    public static void AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfigSection = configuration.GetSection("AppSettings");
        services.Configure<AppConfig>(appConfigSection);

        var appConfig = appConfigSection.Get<AppConfig>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
            x.SaveToken = true;
            x.SetJwksOptions(new JwkOptions(appConfig.AutenticacaoJwksUrl));
        });
    }

    public static void UseAutenticacaoConfig(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
