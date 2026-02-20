using DM.Identidade.API.Data;
using DM.Identidade.API.Extensions;
using DM.Identidade.API.Models;
using DM.WebAPI.Core.Identidade;
using DM.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DM.Identidade.API.Services;

public class AutenticacaoServico
{
    public readonly SignInManager<IdentityUser> SignInManager;
    public readonly UserManager<IdentityUser> UserManager;
    private readonly AppConfig _appSettings;
    private readonly AppTokenConfig _appTokenConfig;
    private readonly AppDbContext _context;
    private readonly IJwtService _jwksService;
    private readonly IUsuario _usuario;

    public AutenticacaoServico(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppConfig> appSettings,
        IOptions<AppTokenConfig> appTokenConfig, AppDbContext context, IJwtService jwksService, IUsuario usuario)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        _appSettings = appSettings.Value;
        _appTokenConfig = appTokenConfig.Value;
        _jwksService = jwksService;
        _usuario = usuario;
        _context = context;
    }

    public async Task<UsuarioRespostaLogin> GerarJwt(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        var claims = await UserManager.GetClaimsAsync(user);
        var identityClaims = await ObterClaimsUsuario(claims, user);
        var encodedToken = CodificarToken(identityClaims);
        var refreshToken = await GerarRefreshToken(email);

        return ObterRespostaToken(encodedToken, user, claims, refreshToken);
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
    {
        var userRoles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var currentIssuer = $"{_usuario.ObterHttpContext().Request.Scheme}://{_usuario.ObterHttpContext().Request.Host}";
        //var key = _jwksService.GetCurrent();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = currentIssuer,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(1),
            //SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user,
        IEnumerable<Claim> claims, RefreshToken refreshToken)
    {
        return new UsuarioRespostaLogin
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);

    private async Task<RefreshToken> GerarRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            Username = email,
            ExpirationDate = DateTime.UtcNow.AddHours(_appTokenConfig.RefreshTokenExpiration)
        };

        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Username == email));
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
    {
        var token = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(u => u.Token == refreshToken);

        return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now ? token : null;
    }
}