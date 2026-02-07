using DM.Pagamentos.API.Data;
using DM.Pagamentos.API.Data.Repository;
using DM.Pagamentos.API.Facade;
using DM.Pagamentos.API.Models;
using DM.Pagamentos.API.Services;
using DM.WebAPI.Core.Usuario;

namespace DM.Pagamentos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, Usuario>();

        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<PagamentosContext>();
    }
}

