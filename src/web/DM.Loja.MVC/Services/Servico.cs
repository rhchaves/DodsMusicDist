using DM.Core.Communication;
using DM.Loja.MVC.Extensions;
using Org.BouncyCastle.Ocsp;
using System.Text;
using System.Text.Json;

namespace DM.Loja.MVC.Services;

public abstract class Servico
{
    protected StringContent ObterConteudo(object dado)
    {
        return new StringContent(JsonSerializer.Serialize(dado), Encoding.UTF8, "application/json");
    }

    protected async Task<T> DeserializarObjetoResposta<T>(HttpResponseMessage responseMessage)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
    }

    protected bool TratarErrosResposta(HttpResponseMessage resposta)
    {
        switch ((int)resposta.StatusCode)
        {
            case 401:
            case 403:
            case 404:
            case 500:

                var teste = resposta;
                var problema = resposta.RequestMessage;

                throw new CustomHttpRequestException(resposta.StatusCode);

            case 400:
                return false;
        }

        resposta.EnsureSuccessStatusCode();
        return true;
    }

    protected ResponseResult RetornoOk()
    {
        return new ResponseResult();
    }
}
