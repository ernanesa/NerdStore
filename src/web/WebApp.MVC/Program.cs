using WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddIdentityConfiguration();
builder.Services.AddWebAppConfiguration(builder.Configuration);
builder.Services.RegisterServices();

var app = builder.Build();

app.UseWebAppConfiguration(app.Environment);

app.Run();