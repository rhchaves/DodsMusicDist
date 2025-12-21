namespace DM.Core.Messages;

public abstract class Mensagem
{
    public string MessageType { get; protected set; }
    public Guid AggregateId { get; protected set; }

    protected Mensagem()
    {
        MessageType = GetType().Name;
    }
}