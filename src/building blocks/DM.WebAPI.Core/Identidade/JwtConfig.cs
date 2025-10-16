using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DM.WebAPI.Core.Identidade;

public static class JwtConfig
{
    public static void AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfigSection = configuration.GetSection("AppSettings");
        services.Configure<AppConfig>(appConfigSection);

        var appConfig = appConfigSection.Get<AppConfig>();
        var key = Encoding.ASCII.GetBytes(appConfig.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = appConfig.ValidoEm,
                ValidIssuer = appConfig.Emissor
            };
        });
    }


    public static void UseAutenticacaoConfig(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
