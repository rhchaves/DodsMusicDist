using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace DM.Core.Messages;

public abstract class Command : Message, IRequest<ValidationResult>
{
    public DateTime Timestamp { get; private set; } = DateTime.Now;
    
    [JsonIgnore]
    public ValidationResult? ValidationResult { get; set; }

    public abstract bool EhValido();
}