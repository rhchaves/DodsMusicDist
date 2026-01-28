using DM.Bff.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface IPagamentoService
{
}

public class PagamentoService : Service, IPagamentoService
{
    private readonly HttpClient _httpClient;

    public PagamentoService(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
    }
}