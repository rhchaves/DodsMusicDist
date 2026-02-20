using DM.Identidade.API.Data;
using DM.WebAPI.Core.DatabaseType;
using Microsoft.AspNetCore.Identity;
using NetDevPack.Identity.Jwt;
using NetDevPack.Security.PasswordHasher.Core;
using static DM.WebAPI.Core.DatabaseType.ProvedorConfig;

namespace DM.Identidade.API.Configuration;

public static class IdentidadeConfig
{
    public static IServiceCollection AddIdentidadeConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurarProvedorParaContexto<AppDbContext>(DetectarBancoDeDados(configuration));

        services.AddMemoryCache().AddDataProtection();

        services.AddJwtConfiguration(configuration, "AppSettings").AddNetDevPackIdentity<IdentityUser>().PersistKeysToDatabaseStore<AppDbContext>();

        services.AddIdentity<IdentityUser, IdentityRole>(set =>
        {
            set.Password.RequireDigit = false;
            set.Password.RequireLowercase = false;
            set.Password.RequireNonAlphanumeric = false;
            set.Password.RequireUppercase = false;
            set.Password.RequiredUniqueChars = 0;
            set.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        services.UpgradePasswordSecurity().WithStrengthen(PasswordHasherStrength.Moderate).UseArgon2<IdentityUser>();

        return services;
    }
}
