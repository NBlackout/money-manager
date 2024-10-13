using Microsoft.Extensions.DependencyInjection;
using Shared.Infra.DateOnlyProvider;
using Shared.Ports;

namespace Shared.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfra(this IServiceCollection services) =>
        services.AddSingleton<IDateOnlyProvider, ReadDateOnlyProvider>();
}