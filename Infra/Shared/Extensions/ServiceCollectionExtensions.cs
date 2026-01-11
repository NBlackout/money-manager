using App.Shared.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfra(this IServiceCollection services) =>
        services.AddSingleton<IClock, SystemClock>().AddSingleton<ICsvHelper, CsvHelper>();
}