using DM.Bff.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface IPagamentoServico
{
}

public class PagamentoServico : Servico, IPagamentoServico
{
    private readonly HttpClient _httpClient;

    public PagamentoServico(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
    }
}