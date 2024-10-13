using Microsoft.Extensions.DependencyInjection;
using Read.Infra;

namespace Read.Api;

public static class ServiceCollectionExtensions
{
    public static void AddRead(this IServiceCollection services) =>
        services.AddReadInfra();
}