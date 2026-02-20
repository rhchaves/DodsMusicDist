using DM.Core.Communication;
using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using DM.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using NetDevPack.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DM.Loja.MVC.Services;

public interface IAutenticacaoServico
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
    Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
    Task RealizarLogin(UsuarioRespostaLogin resposta);
    Task Logout();
    bool TokenExpirado();
    Task<bool> RefreshTokenValido();
}

public class AutenticacaoServico : Servico, IAutenticacaoServico
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IUsuario _usuario;

    public AutenticacaoServico(HttpClient httpClient, IOptions<AppConfig> config, IUsuario usuario, IHttpContextAccessor httpContextAccessor)
    {
        httpClient.BaseAddress = new Uri(config.Value.AutenticacaoUrl);

        _httpClient = httpClient;
        _usuario = usuario;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task RealizarLogin(UsuarioRespostaLogin resposta)
    {
        var token = ObterTokenFormatado(resposta.AccessToken);

        var claims = new List<Claim>
        {
            new("JWT", resposta.AccessToken),
            new ("RefreshToken", resposta.RefreshToken)
        };
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = true
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme,null);
    }

    public bool TokenExpirado()
    {
        var jwt = _usuario.ObterUsuarioToken();
        if (jwt.IsMissing()) return false;

        var token = ObterTokenFormatado(jwt);
        return token.ValidTo.ToLocalTime() < DateTime.Now;
    }

    public async Task<bool> RefreshTokenValido()
    {
        var resposta = await UtilizarRefreshToken(_usuario.ObterUsuarioRefreshToken());

        if (resposta.AccessToken == null || resposta.ResultadoResposta != null) return false;

        await RealizarLogin(resposta);

        return true;
    }

    private async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
    {
        var refreshTokenContent = ObterConteudo(refreshToken);

        var resposta = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

        if (!TratarErrosResposta(resposta))
        {
            return new UsuarioRespostaLogin
            {
                ResultadoResposta = await DeserializarObjetoResposta<ResponseResult>(resposta)
            };
        }

        return await DeserializarObjetoResposta<UsuarioRespostaLogin>(resposta);
    }

    private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
    {
        return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
    }
}
