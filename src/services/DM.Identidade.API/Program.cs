using DM.Identidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentidadeConfig(builder.Configuration);
builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddMessageBusConfig(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfig();
app.UseApiConfig(app.Environment);
app.MapControllers();

app.Run();
