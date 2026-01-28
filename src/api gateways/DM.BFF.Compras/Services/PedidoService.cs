using DM.Bff.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface IPedidoService
{
}

public class PedidoService : Service, IPedidoService
{
    private readonly HttpClient _httpClient;

    public PedidoService(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
    }
}