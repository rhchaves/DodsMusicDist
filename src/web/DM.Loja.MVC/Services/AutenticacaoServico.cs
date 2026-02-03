using DM.Core.Communication;
using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;

namespace DM.Loja.MVC.Services;

public interface IAutenticacaoServico
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
    Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
}

public class AutenticacaoServico : Servico, IAutenticacaoServico
{
    private readonly HttpClient _httpClient;

    public AutenticacaoServico(HttpClient httpClient, IOptions<AppConfig> config)
    {
        httpClient.BaseAddress = new Uri(config.Value.AutenticacaoUrl);

        _httpClient = httpClient;
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
    {
        var loginConteudo = ObterConteudo(usuarioLogin);

        var resposta = await _httpClient.PostAsync("/api/identidade/autenticar", loginConteudo);

        if (!TratarErrosResposta(resposta))
        {
            return new UsuarioRespostaLogin
            {
                ResultadoResposta = await DeserializarObjetoResposta<ResponseResult>(resposta)
            };
        }

        return await DeserializarObjetoResposta<UsuarioRespostaLogin>(resposta);
    }

    public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
    {
        var registroConteudo = ObterConteudo(usuarioRegistro);

        var resposta = await _httpClient.PostAsync("/api/identidade/nova-conta", registroConteudo);

        if (!TratarErrosResposta(resposta))
        {
            return new UsuarioRespostaLogin
            {
                ResultadoResposta = await DeserializarObjetoResposta<ResponseResult>(resposta)
            };
        }

        return await DeserializarObjetoResposta<UsuarioRespostaLogin>(resposta);
    }
}
