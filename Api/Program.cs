using Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServices();

WebApplication app = builder.Build();
app.ConfigureHttpRequestPipeline();
app.Run();