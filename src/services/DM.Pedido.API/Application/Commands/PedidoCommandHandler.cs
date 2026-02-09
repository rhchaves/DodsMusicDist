using DM.Core.Messages;
using DM.Core.Messages.Integration;
using DM.Pedidos.API.Application.DTO;
using DM.Pedidos.API.Application.Events;
using DM.Pedidos.Domain.Pedidos;
using DM.Pedidos.Domain.Vouchers;
using DM.Pedidos.Domain.Vouchers.Especificacao;
using MassTransit;
using MediatR;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DM.Pedidos.API.Application.Commands;

public class PedidoCommandHandler : CommandHandler, IRequestHandler<AdicionarPedidoCommand, ValidationResult>
{
    private readonly IBus _bus;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IVoucherRepository _voucherRepository;

    public PedidoCommandHandler(IVoucherRepository voucherRepository, IPedidoRepository pedidoRepository, IBus bus)
    {
        _voucherRepository = voucherRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        // Validação do comando
        if (!message.EhValido()) return message.ValidationResult;

        // Mapear Pedido
        var pedido = MapearPedido(message);

        // Aplicar voucher se houver
        if (!await AplicarVoucher(message, pedido)) return ValidationResult;

        // Validar pedido
        if (!ValidarPedido(pedido)) return ValidationResult;

        // Processar pagamento
        if (!await ProcessarPagamento(pedido, message)) return ValidationResult;

        // Se pagamento tudo ok!
        pedido.AutorizarPedido();

        // Adicionar Evento
        pedido.AdicionarEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

        // Adicionar Pedido Repositorio
        _pedidoRepository.Adicionar(pedido);

        // Persistir dados de pedido e voucher
        return await PersistirDados(_pedidoRepository.UnitOfWork);
    }

    private Pedido MapearPedido(AdicionarPedidoCommand message)
    {
        var endereco = new Endereco
        {
            Logradouro = message.Endereco.Logradouro,
            Numero = message.Endereco.Numero,
            Complemento = message.Endereco.Complemento,
            Bairro = message.Endereco.Bairro,
            Cep = message.Endereco.Cep,
            Cidade = message.Endereco.Cidade,
            Estado = message.Endereco.Estado
        };

        var pedido = new Pedido(message.ClienteId, message.ValorTotal, message.PedidoItems.Select(PedidoItemDTO.ParaPedidoItem).ToList(),
            message.VoucherUtilizado, message.Desconto);

        pedido.AtribuirEndereco(endereco);
        return pedido;
    }

    private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Pedido pedido)
    {
        if (!message.VoucherUtilizado) return true;

        var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo);
        if (voucher == null)
        {
            AdicionarErro("O voucher informado não existe!");
            return false;
        }

        var voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(m => AdicionarErro(m.ErrorMessage));
            return false;
        }

        pedido.AtribuirVoucher(voucher);
        voucher.DebitarQuantidade();

        _voucherRepository.Atualizar(voucher);

        return true;
    }

    private bool ValidarPedido(Pedido pedido)
    {
        var pedidoValorOriginal = pedido.ValorTotal;
        var pedidoDesconto = pedido.Desconto;

        pedido.CalcularValorPedido();

        if (pedido.ValorTotal != pedidoValorOriginal)
        {
            AdicionarErro("O valor total do pedido não confere com o cálculo do pedido");
            return false;
        }

        if (pedido.Desconto != pedidoDesconto)
        {
            AdicionarErro("O valor total não confere com o cálculo do pedido");
            return false;
        }

        return true;
    }

    public async Task<bool> ProcessarPagamento(Pedido pedido, AdicionarPedidoCommand mensagem)
    {
        var pedidoIniciado = new PedidoIniciadoIntegrationEvent
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Valor = pedido.ValorTotal,
            TipoPagamento = 1, // fixed - change if we have more types
            NomeCartao = mensagem.NomeCartao,
            NumeroCartao = mensagem.NumeroCartao,
            MesAnoVencimento = mensagem.ExpiracaoCartao,
            CVV = mensagem.CvvCartao
        };

        var resultado = await _bus.Request<PedidoIniciadoIntegrationEvent, ResponseMessage>(pedidoIniciado);

        if (resultado.Message.ValidationResult.IsValid) return true;

        foreach (var error in resultado.Message.ValidationResult.Errors) AdicionarErro(error.ErrorMessage);

        return false;
    }
}