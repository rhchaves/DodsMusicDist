using DM.Identidade.API.Data;
using DM.Identidade.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtSigningCredentials;

namespace DM.Identidade.API.Configuration;

public static class IdentidadeConfig
{
    public static IServiceCollection AddIdentidadeConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwksManager(options => options.Algorithm = Algorithm.ES256)
            .PersistKeysToDatabaseStore<AppDbContext>();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<IdentidadeMsgPtBr>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
