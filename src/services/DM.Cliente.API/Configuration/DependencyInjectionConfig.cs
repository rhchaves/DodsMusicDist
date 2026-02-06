using DM.Clientes.API.Models;
using DM.Clientes.API.Application.Commands;
using DM.Clientes.API.Application.Events;
using DM.Clientes.API.Data;
using DM.Clientes.API.Data.Repository;
using DM.Core.Mediator;
using FluentValidation.Results;
using MediatR;
using DM.WebAPI.Core.Usuario;

namespace DM.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, Usuario>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();
        services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
    }
}