using DM.Bff.Compras.Extensions;
using DM.Bff.Compras.Models;
using DM.Core.Communication;
using Microsoft.Extensions.Options;
using System.Net;

namespace DM.Bff.Compras.Services;

public interface IPedidoServico
{
    Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);
    Task<PedidoDTO> ObterUltimoPedido();
    Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();
    Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
}

public class PedidoServico : Servico, IPedidoServico
{
    private readonly HttpClient _httpClient;

    public PedidoServico(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
    }

    public async Task<ResponseResult> FinalizarPedido(PedidoDTO pedido)
    {
        var pedidoContent = ObterConteudo(pedido);

        var resposta = await _httpClient.PostAsync("/pedido/", pedidoContent);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }

    public async Task<PedidoDTO> ObterUltimoPedido()
    {
        var resposta = await _httpClient.GetAsync("/pedido/ultimo/");

        if (resposta.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<PedidoDTO>(resposta);
    }

    public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
    {
        var resposta = await _httpClient.GetAsync("/pedido/lista-cliente/");

        if (resposta.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<IEnumerable<PedidoDTO>>(resposta);
    }

    public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
    {
        var resposta = await _httpClient.GetAsync($"/voucher/{codigo}/");

        if (resposta.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<VoucherDTO>(resposta);
    }
}