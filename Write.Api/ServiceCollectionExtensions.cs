using Microsoft.Extensions.DependencyInjection;
using Write.Infra;

namespace Write.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWrite(this IServiceCollection services) =>
        services.AddWriteInfra();
}