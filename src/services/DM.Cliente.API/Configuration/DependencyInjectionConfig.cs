using DM.Clientes.API.Data;
using DM.Clientes.API.Data.Repository;
using DM.Clientes.API.Models;
using DM.WebAPI.Core.Usuario;

namespace DM.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegistrarServicos(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, Usuario>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
    }
}