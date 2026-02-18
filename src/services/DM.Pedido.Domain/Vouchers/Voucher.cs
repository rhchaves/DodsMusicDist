using DM.Core.DomainObjects;
using DM.Pedidos.Domain.Vouchers.Especificacao;

namespace DM.Pedidos.Domain.Vouchers;

public class Voucher : Entidade, IAggregateRoot
{
    public string Codigo { get; private set; }
    public decimal? Percentual { get; private set; }
    public decimal? ValorDesconto { get; private set; }
    public int Quantidade { get; private set; }
    public TipoDescontoVoucher TipoDesconto { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataUtilizacao { get; private set; }
    public DateTime DataValidade { get; private set; }
    public bool Ativo { get; private set; }
    public bool Utilizado { get; private set; }

    public Voucher(string codigo, decimal? percentual, decimal? valorDesconto, int quantidade, TipoDescontoVoucher tipoDesconto, DateTime dataValidade)
    {
        Codigo = codigo;
        Percentual = percentual;
        ValorDesconto = valorDesconto;
        Quantidade = quantidade;
        TipoDesconto = tipoDesconto;
        DataValidade = dataValidade;

        DataCriacao = DateTime.Now;
        Ativo = true;
        Utilizado = false;
    }

    public bool EstaValidoParaUtilizacao()
    {
        return new VoucherAtivoSpecification()
            .And(new VoucherDataSpecification())
            .And(new VoucherQuantidadeSpecification())
            .IsSatisfiedBy(this);
    }

    public void MarcarComoUtilizado()
    {
        Ativo = false;
        Utilizado = true;
        Quantidade = 0;
        DataUtilizacao = DateTime.Now;
    }

    public void DebitarQuantidade()
    {
        Quantidade -= 1;
        if (Quantidade >= 1) return;

        MarcarComoUtilizado();
    }
}