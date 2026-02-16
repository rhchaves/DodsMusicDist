using DM.Bff.Compras.Models;
using DM.Bff.Compras.Services;
using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Bff.Compras.Controllers;

[Authorize]
[Route("compras/carrinho")]
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
    [Route("")]
    public async Task<IActionResult> Index()
    {
        return ValidarResposta( await _carrinhoServico.ObterCarrinho());
    }

    [HttpGet]
    [Route("quantidade")]
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var quantidade = await _carrinhoServico.ObterCarrinho();
        return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
    }

    [HttpPost]
    [Route("items")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemCarrinho)
    {
        var produto = await _catalogoServico.ObterPorId(itemCarrinho.ProdutoId);

        await ValidarItemCarrinho(produto, itemCarrinho.Quantidade, true);
        if (!OperacaoValida()) return ValidarResposta();

        PreencherDadosDoProduto(itemCarrinho, produto);

        var resposta = await _carrinhoServico.AdicionarItemCarrinho(itemCarrinho);

        return ValidarResposta();
    }

    [HttpPut]
    [Route("items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemCarrinho)
    {
        var produto = await _catalogoServico.ObterPorId(produtoId);

        await ValidarItemCarrinho(produto, itemCarrinho.Quantidade);
        if (!OperacaoValida()) return ValidarResposta();
        
        PreencherDadosDoProduto(itemCarrinho, produto);

        var resposta = await _carrinhoServico.AtualizarItemCarrinho(produtoId, itemCarrinho);

        return ValidarResposta();
    }

    [HttpDelete]
    [Route("items/{produtoId}")]
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
    [Route("aplicar-voucher")]
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

    private static void PreencherDadosDoProduto(ItemCarrinhoDTO itemCarrinho, ItemProdutoDTO itemProduto)
    {
        itemCarrinho.ProdutoId = itemProduto.Id;
        itemCarrinho.Nome = itemProduto.Nome;
        itemCarrinho.Valor = itemProduto.Valor;
        itemCarrinho.Imagem = itemProduto.Imagem;
    }
}
