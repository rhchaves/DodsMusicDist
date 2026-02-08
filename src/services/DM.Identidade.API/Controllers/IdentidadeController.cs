using DM.Core.Messages.Integration;
using DM.Identidade.API.Models;
using DM.Identidade.API.Services;
using DM.MessageBus;
using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DM.Identidade.API.Controllers;

[Route("api/identidade")]
public class IdentidadeController : MainController
{
    AutenticacaoServico _autenticacaoServico;
    private readonly IMessageBus _bus;

    public IdentidadeController(AutenticacaoServico autenticacaoServico, IMessageBus bus)
    {
        _autenticacaoServico = autenticacaoServico;
        _bus = bus;
    }

    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return ValidarResposta(ModelState);

        var usuario = new IdentityUser
        {
            UserName = usuarioRegistro.Email,
            Email = usuarioRegistro.Email,
            EmailConfirmed = true
        };

        var resultado = await _autenticacaoServico.UserManager.CreateAsync(usuario, usuarioRegistro.Senha);

        if (resultado.Succeeded)
        {
            var clienteResultado = await RegistrarCliente(usuarioRegistro);

            if (!clienteResultado.ValidationResult.IsValid)
            {
                await _autenticacaoServico.UserManager.DeleteAsync(usuario);
                return ValidarResposta(clienteResultado.ValidationResult);
            }

            return ValidarResposta(await _autenticacaoServico.GerarJwt(usuarioRegistro.Email));
        }

        foreach (var error in resultado.Errors)
        {
            AdicionarErroProcessamento(error.Description);
        }

        return ValidarResposta();
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid) return ValidarResposta(ModelState);

        var resultado = await _autenticacaoServico.SignInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha,
            false, true);

        if (resultado.Succeeded)
        {
            return ValidarResposta(await _autenticacaoServico.GerarJwt(usuarioLogin.Email));
        }

        if (resultado.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            return ValidarResposta();
        }

        AdicionarErroProcessamento("Usuário ou Senha incorretos");
        return ValidarResposta();
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await _autenticacaoServico.UserManager.FindByEmailAsync(usuarioRegistro.Email);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
            Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch
        {
            await _autenticacaoServico.UserManager.DeleteAsync(usuario);
            throw;
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            AdicionarErroProcessamento("Refresh Token inválido");
            return ValidarResposta();
        }

        var token = await _autenticacaoServico.ObterRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            AdicionarErroProcessamento("Refresh Token expirado");
            return ValidarResposta();
        }

        return ValidarResposta(await _autenticacaoServico.GerarJwt(token.Username));
    }
}
