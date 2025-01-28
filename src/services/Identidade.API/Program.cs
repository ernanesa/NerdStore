
using Identidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseApiConfiguration(builder.Environment);

app.Run();
