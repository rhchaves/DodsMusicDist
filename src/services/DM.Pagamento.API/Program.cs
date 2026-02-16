using DM.Pagamentos.API.Configuration;
using DM.WebAPI.Core.Configuration;
using DM.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddJwtConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.RegistrarServicos();
builder.Services.AddMessageBusConfig(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfig();
app.UseApiCoreConfig(app.Environment);
app.Run();
