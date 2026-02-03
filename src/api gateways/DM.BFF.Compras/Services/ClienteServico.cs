using DM.Bff.Compras.Extensions;
using DM.Bff.Compras.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace DM.Bff.Compras.Services;

public interface IClienteServico
{
    Task<EnderecoDTO> ObterEndereco();
}

public class ClienteServico : Servico, IClienteServico
{
    private readonly HttpClient _httpClient;

    public ClienteServico(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
    }

    public async Task<EnderecoDTO> ObterEndereco()
    {
        var resposta = await _httpClient.GetAsync("/cliente/endereco/");

        if (resposta.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResposta(resposta);

        return await DeserializarObjetoResposta<EnderecoDTO>(resposta);
    }
}