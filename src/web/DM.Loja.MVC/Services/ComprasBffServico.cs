using DM.Core.Communication;
using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;

namespace DM.Loja.MVC.Services;

public interface IComprasBffServico
{
    // Carrinho
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<int> ObterQuantidadeCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(string voucher);

    // Pedido
    Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
    Task<PedidoViewModel> ObterUltimoPedido();
    Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
    PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
}

public class ComprasBffServico : Servico, IComprasBffServico
{
    private readonly HttpClient _httpClient;

    public ComprasBffServico(HttpClient httpClient, IOptions<AppConfig> config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(config.Value.ComprasBffUrl);
    }

    #region Carrinho

    public async Task<CarrinhoViewModel> ObterCarrinho()
    {
        var resposta = await _httpClient.GetAsync("/compras/carrinho/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<CarrinhoViewModel>(resposta);
    }
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var resposta = await _httpClient.GetAsync("/compras/carrinho-quantidade/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<int>(resposta);
    }
    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho)
    {
        var itemConteudo = ObterConteudo(carrinho);

        var resposta = await _httpClient.PostAsync("/compras/carrinho/items/", itemConteudo);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }
    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel item)
    {
        var itemConteudo = ObterConteudo(item);

        var resposta = await _httpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemConteudo);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }
    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var resposta = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }
    public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
    {
        var itemConteudo = ObterConteudo(voucher);

        var resposta = await _httpClient.PostAsync("/compras/carrinho/aplicar-voucher/", itemConteudo);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }

    #endregion

    #region Pedido

    public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
    {
        var pedidoConteudo = ObterConteudo(pedidoTransacao);

        var resposta = await _httpClient.PostAsync("/compras/pedido/", pedidoConteudo);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }

    public async Task<PedidoViewModel> ObterUltimoPedido()
    {
        var resposta = await _httpClient.GetAsync("/compras/pedido/ultimo/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<PedidoViewModel>(resposta);
    }

    public async Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId()
    {
        var resposta = await _httpClient.GetAsync("/compras/pedido/lista-cliente/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<IEnumerable<PedidoViewModel>>(resposta);
    }

    public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco)
    {
        var pedido = new PedidoTransacaoViewModel
        {
            ValorTotal = carrinho.ValorTotal,
            Itens = carrinho.Itens,
            Desconto = carrinho.Desconto,
            VoucherUtilizado = carrinho.VoucherUtilizado,
            VoucherCodigo = carrinho.Voucher?.Codigo
        };

        if (endereco != null)
        {
            pedido.Endereco = new EnderecoViewModel
            {
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Cep = endereco.Cep,
                Complemento = endereco.Complemento,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado
            };
        }

        return pedido;
    }

    #endregion
}