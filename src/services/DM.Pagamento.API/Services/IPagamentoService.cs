using DM.Core.Messages.Integration;
using DM.Pagamentos.API.Models;

namespace DM.Pagamentos.API.Services;

public interface IPagamentoService
{
    Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
    Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
    Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
}
