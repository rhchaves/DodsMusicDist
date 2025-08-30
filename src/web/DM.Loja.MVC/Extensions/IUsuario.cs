
using System.Security.Claims;

namespace DM.Loja.MVC.Extensions;

public interface IUsuario
{
    string Name { get; }
    Guid ObterUsuarioId();
    string ObterUsuarioEmail();
    string ObterUsuarioToken();
    bool EstaAutenticado();
    bool PossuiRole(string role);
    IEnumerable<Claim> ObterClaims();
    HttpContext ObterHttpContext();
}

public class AspNetUser : IUsuario
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
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

public static class ClaimsPrincipalExtensions
{
    public static string GetUsuarioId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("sub");
        return claim?.Value;
    }

    public static string GetUsuarioEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("email");
        return claim?.Value;
    }

    public static string GetUsuarioToken(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("JWT");
        return claim?.Value;
    }
}