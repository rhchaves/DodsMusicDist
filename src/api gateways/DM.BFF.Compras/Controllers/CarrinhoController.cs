using DM.Bff.Compras.Models;
using DM.Bff.Compras.Services;
using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Bff.Compras.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly ICarrinhoServico _carrinhoServico;
    private readonly ICatalogoServico _catalogoServico;
    private readonly IPedidoServico _pedidoServico;

    public CarrinhoController(ICarrinhoServico carrinhoServico, ICatalogoServico catalogoServico, IPedidoServico pedidoServico)
    {
        _carrinhoServico = carrinhoServico;
        _catalogoServico = catalogoServico;
        _pedidoServico = pedidoServico;
    }

    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index()
    {
        return ValidarResposta();
    }

    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var quantidade = await _carrinhoServico.ObterCarrinho();
        return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
    }

    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemProduto)
    {
        var produto = await _catalogoServico.ObterPorId(itemProduto.ProdutoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade, true);
        if (!OperacaoValida()) return ValidarResposta();

        itemProduto.Nome = produto.Nome;
        itemProduto.Valor = produto.Valor;
        itemProduto.Imagem = produto.Imagem;

        var resposta = await _carrinhoServico.AdicionarItemCarrinho(itemProduto);

        return ValidarResposta();
    }

    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemProduto)
    {
        var produto = await _catalogoServico.ObterPorId(produtoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade);
        if (!OperacaoValida()) return ValidarResposta();

        var resposta = await _carrinhoServico.AtualizarItemCarrinho(produtoId, itemProduto);

        return ValidarResposta();
    }

    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var produto = await _catalogoServico.ObterPorId(produtoId);

        if (produto == null)
        {
            AdicionarErroProcessamento("Produto inexistente!");
            return ValidarResposta();
        }

        var resposta = await _carrinhoServico.RemoverItemCarrinho(produtoId);

        return ValidarResposta();
    }

    [HttpPost]
    [Route("compras/carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
    {
        var voucher = await _pedidoServico.ObterVoucherPorCodigo(voucherCodigo);
        if (voucher is null)
        {
            AdicionarErroProcessamento("Voucher inválido ou não encontrado!");
            return ValidarResposta();
        }

        var resposta = await _carrinhoServico.AplicarVoucherCarrinho(voucher);

        return ValidarResposta(resposta);
    }

    private async Task ValidarItemCarrinho(ItemProdutoDTO produto, int quantidade, bool adicionarProduto = false)
    {
        if (produto == null) AdicionarErroProcessamento("Produto inexistente!");
        if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto.Nome}");

        var carrinho = await _carrinhoServico.ObterCarrinho();
        var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto.Id);

        if (itemCarrinho != null && adicionarProduto && itemCarrinho.Quantidade + quantidade > produto.QuantidadeEstoque)
        {
            AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
            return;
        }

        if (quantidade > produto.QuantidadeEstoque) AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }
}
