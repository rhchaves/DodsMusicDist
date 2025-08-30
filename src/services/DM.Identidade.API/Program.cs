using DM.Identidade.API.Configuration;

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

builder.Services.AddIdentidadeConfig(builder.Configuration);
builder.Services.AddApiConfig();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseApiConfig(app.Environment);

app.Run();
