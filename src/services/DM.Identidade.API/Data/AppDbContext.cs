using DM.Identidade.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtSigningCredentials;
using NetDevPack.Security.JwtSigningCredentials.Store.EntityFrameworkCore;

namespace DM.Identidade.API.Data;

public class AppDbContext : IdentityDbContext, ISecurityKeyContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
