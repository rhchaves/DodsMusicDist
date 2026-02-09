using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DM.WebAPI.Core.DatabaseType;

public static class ContextoConfig
{
    /// <summary>
    ///     ASP.NET Identity Context config
    /// </summary>
    public static IServiceCollection PersistDados<TContext>(this IServiceCollection services,
        Action<DbContextOptionsBuilder> databaseConfig) where TContext : DbContext
    {
        // Add a DbContext to store Keys. SigningCredentials and DataProtectionKeys
        if (services.All(x => x.ServiceType != typeof(TContext)))
            services.AddDbContext<TContext>(databaseConfig);
        return services;
    }
}