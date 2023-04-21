using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MoneyManager.Shared.TestTooling;

public static class HostExtensions
{
    public static TImplementation GetRequiredService<TService, TImplementation>(this IHost host)
        where TService : class
        where TImplementation : class, TService =>
        (TImplementation)host.Services.GetRequiredService<TService>();
}