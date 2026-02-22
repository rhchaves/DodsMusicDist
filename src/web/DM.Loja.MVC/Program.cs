using DM.Loja.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentidadeConfig();
builder.Services.AddMvcConfiguracao(builder.Configuration);
builder.Services.RegistrarServicos(builder.Configuration);

var app = builder.Build();

app.UseMvcConfiguracao(app.Environment);

app.Run();
