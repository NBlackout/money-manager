using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Api.Configuration;

public class ConfigureSwagger : IConfigureOptions<SwaggerGenOptions>, IConfigureOptions<SwaggerOptions>, IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerGenOptions options) =>
        options.SwaggerDoc("api", new OpenApiInfo { Title = "API" });

    public void Configure(SwaggerOptions options) =>
        options.RouteTemplate = "api/swagger/{documentName}.json";

    public void Configure(SwaggerUIOptions options)
    {
        options.RoutePrefix = "api/swagger";

        options.EnableTryItOutByDefault();

        options.SwaggerEndpoint("api.json", "API");
    }
}