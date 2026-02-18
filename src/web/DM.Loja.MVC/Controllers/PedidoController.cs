using DM.Loja.MVC.Models;
using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers;

public class PedidoController : MainController
{
    private readonly IClienteServico _clienteServico;
    private readonly IComprasBffServico _comprasBffServico;

    public PedidoController(IClienteServico clienteServico, IComprasBffServico comprasBffServico)
    {
        _clienteServico = clienteServico;
        _comprasBffServico = comprasBffServico;
    }

    [HttpGet]
    [Route("endereco-de-entrega")]
    public async Task<IActionResult> EnderecoEntrega()
    {
        var carrinho = await _comprasBffServico.ObterCarrinho();
        if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

        var endereco = await _clienteServico.ObterEndereco();
        var pedido = _comprasBffServico.MapearParaPedido(carrinho, endereco);

        return View(pedido);
    }

    [HttpGet]
    [Route("pagamento")]
    public async Task<IActionResult> Pagamento()
    {
        var carrinho = await _comprasBffServico.ObterCarrinho();
        if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

        var pedido = _comprasBffServico.MapearParaPedido(carrinho, null);

        return View(pedido);
    }

    [HttpPost]
    [Route("finalizar-pedido")]
    public async Task<IActionResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
    {
        if (!ModelState.IsValid) return View("Pagamento", _comprasBffServico.MapearParaPedido(
            await _comprasBffServico.ObterCarrinho(), null));

        var retorno = await _comprasBffServico.FinalizarPedido(pedidoTransacao);

        if (RespostaPossuiErros(retorno))
        {
            var carrinho = await _comprasBffServico.ObterCarrinho();
            if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var pedidoMap = _comprasBffServico.MapearParaPedido(carrinho, null);
            return View("Pagamento", pedidoMap);
        }

        return RedirectToAction("PedidoConcluido");
    }

    [HttpGet]
    [Route("pedido-concluido")]
    public async Task<IActionResult> PedidoConcluido()
    {
        return View("PedidoConfirmado", await _comprasBffServico.ObterUltimoPedido());
    }

    [HttpGet("meus-pedidos")]
    public async Task<IActionResult> MeusPedidos()
    {
        return View(await _comprasBffServico.ObterListaPorClienteId());
    }
}
