using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DM.WebAPI.Core.Usuario;

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
