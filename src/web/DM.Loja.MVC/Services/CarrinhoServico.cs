using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;

namespace DM.Loja.MVC.Services;

public class CarrinhoServico : Servico, ICarrinhoServico
{
    private readonly HttpClient _httpClient;

    public CarrinhoServico(HttpClient httpClient, IOptions<AppConfig> config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(config.Value.CarrinhoUrl);
    }

    public async Task<CarrinhoViewModel> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/carrinho/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
    }

    public async Task<ResultadoResposta> AdicionarItemCarrinho(ItemProdutoViewModel produto)
    {
        var itemContent = ObterConteudo(produto);

        var response = await _httpClient.PostAsync("/carrinho/", itemContent);

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResultadoResposta>(response);

        return RetornoOk();
    }

    public async Task<ResultadoResposta> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto)
    {
        var itemContent = ObterConteudo(produto);

        var response = await _httpClient.PutAsync($"/carrinho/{produto.ProdutoId}", itemContent);

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResultadoResposta>(response);

        return RetornoOk();
    }

    public async Task<ResultadoResposta> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResultadoResposta>(response);

        return RetornoOk();
    }
}