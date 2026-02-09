using DM.Clientes.API.Models;
using DM.Core.Data;
using DM.Core.DomainObjects;
using DM.Core.Messages;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DM.Clientes.API.Data;

public sealed class ClientesContext : DbContext, IUnitOfWork
{
    //private readonly IMediatorHandler _mediator;
    private readonly IMediator _mediator;

    public ClientesContext(DbContextOptions<ClientesContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        var sucesso = await base.SaveChangesAsync() > 0;
        if (sucesso) await _mediator.PublicarEventos(this);

        return sucesso;
    }
}

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediator mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entidade>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notificacoes)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        //var tasks = domainEvents
        //    .Select(async (domainEvent) =>
        //    {
        //        await mediator.PublicarEvento(domainEvent);
        //    });

        //await Task.WhenAll(tasks);
        foreach (var task in domainEvents) await mediator.Publish(task);
    }
}