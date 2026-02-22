using DM.Core.Messages.Integration;
using DM.Identidade.API.Models;
using DM.WebAPI.Core.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Identity.Interfaces;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace DM.Identidade.API.Controllers;

[Route("api/identidade")]
public class IdentidadeController : MainController
{
    private readonly IBus _bus;
    private readonly IJwtBuilder _jwtBuilder;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public IdentidadeController(IBus bus, IJwtBuilder jwtBuilder, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _bus = bus;
        _jwtBuilder = jwtBuilder;
        _signInManager = signInManager;
        _userManager = userManager;
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

        var resultado = await _userManager.CreateAsync(usuario, usuarioRegistro.Senha);

        if (resultado.Succeeded)
        {
            var clienteResultado = await RegistrarCliente(usuarioRegistro);

            if (!clienteResultado.ValidationResult.IsValid)
            {
                await _userManager.DeleteAsync(usuario);
                return ValidarResposta(clienteResultado.ValidationResult);
            }

            var jwt = await _jwtBuilder
                .WithEmail(usuarioRegistro.Email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .WithRefreshToken()
                .BuildUserResponse();

            return ValidarResposta(jwt);
        }

        foreach (var error in resultado.Errors) AdicionarErroProcessamento(error.Description);

        return ValidarResposta();
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid) return ValidarResposta(ModelState);

        var resultado = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

        if (resultado.Succeeded)
        {
            var jwt = await _jwtBuilder
                .WithEmail(usuarioLogin.Email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .WithRefreshToken()
                .BuildUserResponse();

            return ValidarResposta(jwt);
        }

        if (resultado.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            return ValidarResposta();
        }

        AdicionarErroProcessamento("Usuário ou Senha incorretos");
        return ValidarResposta();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            AdicionarErroProcessamento("Refresh Token inválido");
            return ValidarResposta();
        }

        var token = await _jwtBuilder.ValidateRefreshToken(refreshToken);

        if (!token.IsValid)
        {
            AdicionarErroProcessamento("Refresh Token expirado");
            return ValidarResposta();
        }

        var jwt = await _jwtBuilder
            .WithUserId(token.UserId)
            .WithJwtClaims()
            .WithUserClaims()
            .WithUserRoles()
            .WithRefreshToken()
            .BuildUserResponse();

        return ValidarResposta(jwt);
    }

#if DEBUG
    [HttpPost("validar-jwt")]
    public async Task<ActionResult> ValidarJwt([FromServices] IJwtService jwtService, [FromForm] string jwt)
    {
        var handler = new JsonWebTokenHandler();

        var teste = await jwtService.GetCurrentSecurityKey();

        var result = await handler.ValidateTokenAsync(jwt, new TokenValidationParameters
        {
            ValidIssuer = "https://localhost:5101",
            ValidAudience = "DodsMusic",
            ValidateAudience = true,
            ValidateIssuer = true,
            RequireSignedTokens = false,
            IssuerSigningKey = teste
        });

        if (!result.IsValid)
            return BadRequest();

        return Ok(result.Claims.Select(s => new { s.Key, s.Value }));
    }

#endif

    #region Métodos Privados
    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);
        ArgumentNullException.ThrowIfNull(usuarioRegistro);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
            Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            var resposta = await _bus.Request<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            return resposta.Message;
        }
        catch (Exception)
        {
            await _userManager.DeleteAsync(usuario);
            throw;
        }
    }
    #endregion
}
