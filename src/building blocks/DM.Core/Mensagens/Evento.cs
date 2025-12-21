using MediatR;

namespace DM.Core.Messages;

public class Evento : Mensagem, INotification
{
    public DateTime Timestamp { get; private set; }

    protected Evento()
    {
        Timestamp = DateTime.Now;
    }
}
