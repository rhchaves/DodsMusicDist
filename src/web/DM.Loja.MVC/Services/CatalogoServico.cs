using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;

namespace DM.Loja.MVC.Services;

public interface ICatalogoServico
{
    Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null);
    Task<ProdutoViewModel> ObterPorId(Guid id);
}

public class CatalogoServico : Servico, ICatalogoServico
{
    private readonly HttpClient _httpClient;

    public CatalogoServico(HttpClient httpClient, IOptions<AppConfig> config)
    {
        httpClient.BaseAddress = new Uri(config.Value.CatalogoUrl);

        _httpClient = httpClient;
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        var resposta = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<ProdutoViewModel>(resposta);
    }

    public async Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null)
    {
        var response = await _httpClient.GetAsync($"/catalogo/produtos?ps={pageSize}&page={pageIndex}&q={query}");

        TratarErrosResposta(response);

        return await DeserializarObjetoResposta<PagedViewModel<ProdutoViewModel>>(response);
    }
}