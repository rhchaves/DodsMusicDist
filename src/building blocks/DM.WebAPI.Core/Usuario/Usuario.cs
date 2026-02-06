using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DM.WebAPI.Core.Usuario;

public class Usuario : IUsuario
{
    private readonly IHttpContextAccessor _accessor;

    public Usuario(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public Guid ObterUsuarioId()
    {
        return EstaAutenticado() ? Guid.Parse(_accessor.HttpContext.User.GetUsuarioId()) : Guid.Empty;
    }

    public string ObterUsuarioEmail()
    {
        return EstaAutenticado() ? _accessor.HttpContext.User.GetUsuarioEmail() : "";
    }

    public string ObterUsuarioToken()
    {
        return EstaAutenticado() ? _accessor.HttpContext.User.GetUsuarioToken() : "";
    }

    public bool EstaAutenticado()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public bool PossuiRole(string role)
    {
        return _accessor.HttpContext.User.IsInRole(role);
    }

    public IEnumerable<Claim> ObterClaims()
    {
        return _accessor.HttpContext.User.Claims;
    }

    public HttpContext ObterHttpContext()
    {
        return _accessor.HttpContext;
    }
}
