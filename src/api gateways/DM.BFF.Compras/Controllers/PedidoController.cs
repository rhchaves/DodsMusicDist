using DM.Bff.Compras.Models;
using DM.Bff.Compras.Services;
using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DM.Bff.Compras.Controllers
{
    [Authorize]
    public class PedidoController : MainController
    {
        private readonly ICatalogoServico _catalogoServico;
        private readonly ICarrinhoServico _carrinhoServico;
        private readonly IPedidoServico _pedidoServico;
        private readonly IClienteServico _clienteServico;

        public PedidoController(ICatalogoServico catalogoServico, ICarrinhoServico carrinhoServico, IPedidoServico pedidoServico,
            IClienteServico clienteServico)
        {
            _catalogoServico = catalogoServico;
            _carrinhoServico = carrinhoServico;
            _pedidoServico = pedidoServico;
            _clienteServico = clienteServico;
        }

        [HttpPost]
        [Route("compras/pedido")]
        public async Task<IActionResult> AdicionarPedido(PedidoDTO pedido)
        {
            var carrinho = await _carrinhoServico.ObterCarrinho();
            var produtos = await _catalogoServico.ObterItens(carrinho.Itens.Select(p => p.ProdutoId));
            var endereco = await _clienteServico.ObterEndereco();

            if (!await ValidarCarrinhoProdutos(carrinho, produtos)) return ValidarResposta();

            PopularDadosPedido(carrinho, endereco, pedido);

            return ValidarResposta(await _pedidoServico.FinalizarPedido(pedido));
        }

        [HttpGet("compras/pedido/ultimo")]
        public async Task<IActionResult> UltimoPedido()
        {
            var pedido = await _pedidoServico.ObterUltimoPedido();
            if (pedido is null)
            {
                AdicionarErroProcessamento("Pedido não encontrado!");
                return ValidarResposta();
            }

            return ValidarResposta(pedido);
        }

        [HttpGet("compras/pedido/lista-cliente")]
        public async Task<IActionResult> ListaPorCliente()
        {
            var pedidos = await _pedidoServico.ObterListaPorClienteId();

            return pedidos == null ? NotFound() : ValidarResposta(pedidos);
        }

        private async Task<bool> ValidarCarrinhoProdutos(CarrinhoDTO carrinho, IEnumerable<ItemProdutoDTO> produtos)
        {
            if (carrinho.Itens.Count != produtos.Count())
            {
                var itensIndisponiveis = carrinho.Itens.Select(c => c.ProdutoId).Except(produtos.Select(p => p.Id)).ToList();

                foreach (var itemId in itensIndisponiveis)
                {
                    var itemCarrinho = carrinho.Itens.FirstOrDefault(c => c.ProdutoId == itemId);
                    AdicionarErroProcessamento($"O item {itemCarrinho.Nome} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }

                return false;
            }

            foreach (var itemCarrinho in carrinho.Itens)
            {
                var produtoCatalogo = produtos.FirstOrDefault(p => p.Id == itemCarrinho.ProdutoId);

                if (produtoCatalogo.Valor != itemCarrinho.Valor)
                {
                    var msgErro = $"O produto {itemCarrinho.Nome} mudou de valor (de: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCarrinho.Valor)} para: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", produtoCatalogo.Valor)}) desde que foi adicionado ao carrinho.";

                    AdicionarErroProcessamento(msgErro);

                    var respostaRemover = await _carrinhoServico.RemoverItemCarrinho(itemCarrinho.ProdutoId);
                    if (RespostaPossuiErros(respostaRemover))
                    {
                        AdicionarErroProcessamento($"Não foi possível remover automaticamente o produto {itemCarrinho.Nome} do seu carrinho, _" +
                                                   "remova e adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    itemCarrinho.Valor = produtoCatalogo.Valor;
                    var respostaAdicionar = await _carrinhoServico.AdicionarItemCarrinho(itemCarrinho);

                    if (RespostaPossuiErros(respostaAdicionar))
                    {
                        AdicionarErroProcessamento($"Não foi possível atualizar automaticamente o produto {itemCarrinho.Nome} do seu carrinho, _" +
                                                   "adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    LimparErrosProcessamento();
                    AdicionarErroProcessamento(msgErro + " Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }

        private void PopularDadosPedido(CarrinhoDTO carrinho, EnderecoDTO endereco, PedidoDTO pedido)
        {
            pedido.VoucherCodigo = carrinho.Voucher?.Codigo;
            pedido.VoucherUtilizado = carrinho.VoucherUtilizado;
            pedido.ValorTotal = carrinho.ValorTotal;
            pedido.Desconto = carrinho.Desconto;
            pedido.PedidoItems = carrinho.Itens;
            pedido.Endereco = endereco;
        }
    }
}
