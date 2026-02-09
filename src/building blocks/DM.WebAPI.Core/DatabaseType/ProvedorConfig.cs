using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DM.WebAPI.Core.DatabaseType;

public class ProvedorConfig
{
    private static readonly string MigrationAssembly = typeof(ProvedorConfig).GetTypeInfo().Assembly.GetName().Name;

    private readonly string _connectionString;

    public ProvedorConfig(string connString)
    {
        _connectionString = connString;
    }

    public Action<DbContextOptionsBuilder> SqlServer =>
        options => options.UseSqlServer(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public Action<DbContextOptionsBuilder> MySql =>
        options => options.UseMySQL(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public Action<DbContextOptionsBuilder> Postgre =>
        options =>
        {
            options.UseNpgsql(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        };

    public Action<DbContextOptionsBuilder> Sqlite =>
        options => options.UseSqlite(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public ProvedorConfig UseCom()
    {
        return this;
    }

    public static ProvedorConfig Build(string connString)
    {
        return new ProvedorConfig(connString);
    }


    /// <summary>
    ///     it's just a tuple. Returns 2 parameters.
    ///     Trying to improve readability at ConfigureServices
    /// </summary>
    public static (TipoBancoDeDados, string) DetectarBancoDeDados(IConfiguration configuration)
    {
        return (
            configuration.GetValue("AppSettings:DatabaseType", TipoBancoDeDados.None),
            configuration.GetConnectionString("DefaultConnection"));
    }
}