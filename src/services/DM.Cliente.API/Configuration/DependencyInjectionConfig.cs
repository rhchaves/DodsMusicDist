using DM.Clientes.API.Models;
using DM.Clientes.API.Application.Commands;
using DM.Clientes.API.Application.Events;
using DM.Clientes.API.Data;
using DM.Clientes.API.Data.Repository;
using DM.Core.Mediator;
using FluentValidation.Results;
using MediatR;

namespace DM.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
        services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
    }
}