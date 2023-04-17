namespace MoneyManager.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSwaggerDependencies()
            .AddBlazorDependencies()
            .AddWriteDependencies()
            .AddReadDependencies();
    }
}