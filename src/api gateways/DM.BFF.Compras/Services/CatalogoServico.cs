using DM.Bff.Compras.Extensions;
using DM.Bff.Compras.Models;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface ICatalogoServico
{
    Task<ItemProdutoDTO> ObterPorId(Guid id);
    Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids);
}

public class CatalogoServico : Servico, ICatalogoServico
{
    private readonly HttpClient _httpClient;

    public CatalogoServico(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
    }

    public async Task<ItemProdutoDTO> ObterPorId(Guid id)
    {
        var resposta = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<ItemProdutoDTO>(resposta);
    }

    public async Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids)
    {
        var idsRequest = string.Join(",", ids);

        var resposta = await _httpClient.GetAsync($"/catalogo/produtos/lista/{idsRequest}/");

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<IEnumerable<ItemProdutoDTO>>(resposta);
    }
}