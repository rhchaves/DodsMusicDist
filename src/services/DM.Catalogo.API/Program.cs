using DM.Catalogo.API.Configuration;
using DM.WebAPI.Core.Configuration;
using DM.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddMessageBusConfig(builder.Configuration);
builder.Services.AddJwtConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.RegistrarServicos();

var app = builder.Build();

app.UseSwaggerConfig();
app.UseApiCoreConfig(app.Environment);
app.Run();

namespace DM.Catalogo.API
{
    public partial class Program;
}