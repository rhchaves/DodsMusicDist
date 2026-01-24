using System.Security.Claims;

namespace DM.WebAPI.Core.Usuario;

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