using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static DM.WebAPI.Core.DatabaseType.ProvedorConfig;

namespace DM.WebAPI.Core.DatabaseType;

public static class SeletorProvedor
{
    public static IServiceCollection ConfigurarProvedorParaContexto<TContext>(this IServiceCollection services,
        (TipoBancoDeDados, string) options) where TContext : DbContext
    {
        var (database, connString) = options;
        return database switch
        {
            TipoBancoDeDados.SqlServer => services.PersistDados<TContext>(Build(connString).UseCom().SqlServer),
            TipoBancoDeDados.MySql => services.PersistDados<TContext>(Build(connString).UseCom().MySql),
            TipoBancoDeDados.Postgre => services.PersistDados<TContext>(Build(connString).UseCom().Postgre),
            TipoBancoDeDados.Sqlite => services.PersistDados<TContext>(Build(connString).UseCom().Sqlite),

            _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
        };
    }

    public static Action<DbContextOptionsBuilder> UseProvedorAutoSelecao((TipoBancoDeDados, string) options)
    {
        var (database, connString) = options;
        return database switch
        {
            TipoBancoDeDados.SqlServer => Build(connString).UseCom().SqlServer,
            TipoBancoDeDados.MySql => Build(connString).UseCom().MySql,
            TipoBancoDeDados.Postgre => Build(connString).UseCom().Postgre,
            TipoBancoDeDados.Sqlite => Build(connString).UseCom().Sqlite,

            _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
        };
    }
}
