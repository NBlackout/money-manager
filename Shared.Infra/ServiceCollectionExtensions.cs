using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfra(this IServiceCollection services) =>
        services.AddSingleton<IClock, SystemClock>();
}