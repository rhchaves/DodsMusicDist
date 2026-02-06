using DM.Core.Mediator;
using DM.Pedidos.API.Application.Commands;
using DM.Pedidos.API.Application.Events;
using DM.Pedidos.API.Application.Queries;
using DM.Pedidos.Domain.Pedidos;
using DM.Pedidos.Domain.Vouchers;
using DM.Pedidos.Infra.Data;
using DM.Pedidos.Infra.Data.Repository;
using DM.WebAPI.Core.Usuario;
using FluentValidation.Results;
using MediatR;

namespace DM.Pedidos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        // API
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, Usuario>();

        // Commands
        services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandHandler>();

        // Events
        services.AddScoped<INotificationHandler<PedidoRealizadoEvent>, PedidoEventHandler>();

        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IVoucherQueries, VoucherQueries>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();

        // Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<PedidosContext>();
    }
}
