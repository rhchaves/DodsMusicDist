using DM.Bff.Compras.Configuration;
using DM.Bff.Compras.Configuration.Configuration;
using DM.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

var hostEnvironment = builder.Environment;

builder.Configuration
    .SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddJwtConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.RegistrarServicos();
builder.Services.AddMessageBusConfig(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfig();
app.UseApiConfig(app.Environment);

app.Run();
