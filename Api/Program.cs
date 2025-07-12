using Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configure();

WebApplication app = builder.Build();
app.Configure();
app.Run();