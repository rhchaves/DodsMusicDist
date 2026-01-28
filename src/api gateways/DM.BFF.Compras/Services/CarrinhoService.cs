using DM.Bff.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface ICarrinhoService
{
}

public class CarrinhoService : Service, ICarrinhoService
{
    private readonly HttpClient _httpClient;

    public CarrinhoService(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
    }
}