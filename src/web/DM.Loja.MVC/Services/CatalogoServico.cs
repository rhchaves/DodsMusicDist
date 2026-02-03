using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;

namespace DM.Loja.MVC.Services;

public interface ICatalogoServico
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
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

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        var resposta = await _httpClient.GetAsync("/catalogo/produtos/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<IEnumerable<ProdutoViewModel>>(resposta);
    }
}