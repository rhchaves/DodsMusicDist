using DM.Bff.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DM.Bff.Compras.Services;

public interface ICatalogoService
{
}

public class CatalogoService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogoService(HttpClient httpClient, IOptions<AppServicesConfig> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
    }
}