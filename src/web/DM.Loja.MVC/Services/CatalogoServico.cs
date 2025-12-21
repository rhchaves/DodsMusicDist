using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;

namespace DM.Loja.MVC.Services;

public class CatalogoServico : Servico, ICatalogoServico
{
    private readonly HttpClient _httpClient;

    public CatalogoServico(HttpClient httpClient,
        IOptions<AppConfig> settings)
    {
        httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);

        _httpClient = httpClient;
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<ProdutoViewModel>(response);
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        var response = await _httpClient.GetAsync("/catalogo/produtos/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
    }
}