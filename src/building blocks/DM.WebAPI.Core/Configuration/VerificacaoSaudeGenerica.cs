using DM.WebAPI.Core.DatabaseType;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static DM.WebAPI.Core.DatabaseType.ProvedorConfig;

namespace DM.WebAPI.Core.Configuration;

public static class VerificacaoSaudeGenerica
{
    public static IHealthChecksBuilder AddVerificacaoSaudeGenerica(this IServiceCollection services, IConfiguration configuration)
    {
        var checkBuilder = services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "api" });

        var (database, connString) = DetectarBancoDeDados(configuration);
        return database switch
        {
            TipoBancoDeDados.SqlServer => checkBuilder.AddSqlServer(connString, name: "SqlServer", tags: new[] { "infra" }),
            TipoBancoDeDados.MySql => checkBuilder.AddMySql(connString, name: "MySql", tags: new[] { "infra" }),
            TipoBancoDeDados.Postgre => checkBuilder.AddNpgSql(connString, name: "Postgre", tags: new[] { "infra" }),
            TipoBancoDeDados.Sqlite => checkBuilder.AddSqlite(connString, name: "Sqlite", tags: new[] { "infra" }),
            _ => checkBuilder
        };
    }

    public static IApplicationBuilder UseVerificacaoSaudeGenerica(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("api"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHealthChecks("/healthz-infra", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("infra"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}