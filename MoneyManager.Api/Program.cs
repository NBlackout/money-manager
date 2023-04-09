using MoneyManager.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

var app = builder.Build();
app.ConfigureHttpRequestPipeline();
app.Run();