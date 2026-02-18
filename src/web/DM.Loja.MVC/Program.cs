using DM.Loja.MVC.Configuration;

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

builder.Services.AddIdentidadeConfig();
builder.Services.AddMvcConfiguracao(builder.Configuration);
builder.Services.RegistrarServicos(builder.Configuration);

var app = builder.Build();

app.UseMvcConfiguracao(app.Environment);

app.Run();
