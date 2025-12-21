using DM.Core.Messages;

namespace DM.Core.DomainObjects;

public abstract class Entidade
{
    public Guid Id { get; set; }

    protected Entidade()
    {
        Id = Guid.NewGuid();
    }

    private List<Evento> _notificacoes;
    public IReadOnlyCollection<Evento> Notificacoes => _notificacoes?.AsReadOnly();

    public void AdicionarEvento(Evento evento)
    {
        _notificacoes = _notificacoes ?? new List<Evento>();
        _notificacoes.Add(evento);
    }

    public void RemoverEvento(Evento eventItem)
    {
        _notificacoes?.Remove(eventItem);
    }

    public void LimparEventos()
    {
        _notificacoes?.Clear();
    }

    #region Comparações

    public override bool Equals(object obj)
    {
        var compareTo = obj as Entidade;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entidade a, Entidade b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entidade a, Entidade b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    #endregion
}