using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Services;
using DM.Loja.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace DM.Loja.MVC.Configuration;

public static class InjecaoDependenciaConfig
{
    public static void RegistrarServicos(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAutenticacaoServico, AutenticacaoServico>();

        services.AddHttpClient<ICatalogoServico, CatalogoServico>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //.AddTransientHttpErrorPolicy(
            //p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUsuario, AspNetUser>();

        #region Refit

        //services.AddHttpClient("Refit",
        //        options =>
        //        {
        //            options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
        //        })
        //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
        //    .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);

        #endregion
    }
}

public class PollyExtensions
{
    public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
    {
        var retry = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            }, (outcome, timespan, retryCount, context) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Tentando pela {retryCount} vez!");
                Console.ForegroundColor = ConsoleColor.White;
            });

        return retry;
    }
}
