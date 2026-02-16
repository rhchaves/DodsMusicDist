using DM.Loja.MVC.Models;
using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers
{
    [Authorize]
    [Route("carrinho")]
    public class CarrinhoController : MainController
    {
        private readonly IComprasBffServico _comprasBffServico;

        public CarrinhoController(IComprasBffServico comprasBffServico)
        {
            _comprasBffServico = comprasBffServico;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _comprasBffServico.ObterCarrinho());
        }

        [HttpPost]
        [Route("adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
        {
            var resposta = await _comprasBffServico.AdicionarItemCarrinho(itemCarrinho);

            if (RespostaPossuiErros(resposta)) return View("Index", await _comprasBffServico.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var itemCarrinho = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            var resposta = await _comprasBffServico.AtualizarItemCarrinho(produtoId, itemCarrinho);

            if (RespostaPossuiErros(resposta)) return View("Index", await _comprasBffServico.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var resposta = await _comprasBffServico.RemoverItemCarrinho(produtoId);

            if (RespostaPossuiErros(resposta)) return View("Index", await _comprasBffServico.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var resposta = await _comprasBffServico.AplicarVoucherCarrinho(voucherCodigo);

            if (RespostaPossuiErros(resposta)) return View("Index", await _comprasBffServico.ObterCarrinho());

            return RedirectToAction("Index");
        }
    }
}