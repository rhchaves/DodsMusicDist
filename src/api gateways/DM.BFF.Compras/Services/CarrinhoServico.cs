using DM.Bff.Compras.Extensions;
using DM.Bff.Compras.Models;
using DM.Core.Communication;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface ICarrinhoServico
{
    Task<CarrinhoDTO> ObterCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher);
}

public class CarrinhoServico : Servico, ICarrinhoServico
{
    private readonly HttpClient _httpClient;

    public CarrinhoServico(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
    }

    public async Task<CarrinhoDTO> ObterCarrinho()
    {
        var resposta = await _httpClient.GetAsync("/carrinho/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<CarrinhoDTO>(resposta);
    }

    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto)
    {
        var itemContent = ObterConteudo(produto);

        var resposta = await _httpClient.PostAsync("/carrinho/", itemContent);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }

    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho)
    {
        var itemContent = ObterConteudo(carrinho);

        var resposta = await _httpClient.PutAsync($"/carrinho/{carrinho.ProdutoId}", itemContent);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }

    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var resposta = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }

    public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher)
    {
        var itemContent = ObterConteudo(voucher);

        var resposta = await _httpClient.PostAsync("/carrinho/aplicar-voucher/", itemContent);

        if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

        return RetornoOk();
    }
}