using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Shared.TestTooling;

public static class DependencyInjectionExtensions
{
    public static TImplementation Resolve<TService, TImplementation>(this IHost host)
        where TService : class
        where TImplementation : class, TService =>
        (TImplementation)host.Services.Resolve<TService>();

    public static TOptions ResolveOptions<TOptions>(this IHost host) where TOptions : class =>
        host.Services.Resolve<IOptions<TOptions>>().Value;

    public static TContract Resolve<TContract>(this IServiceScope scope) where TContract : notnull =>
        scope.ServiceProvider.Resolve<TContract>();

    public static TImplementation Resolve<TContract, TImplementation>(this IServiceScope scope)
        where TContract : notnull
        where TImplementation : TContract =>
        scope.ServiceProvider.Resolve<TContract, TImplementation>();

    private static TContract Resolve<TContract>(this IServiceProvider serviceProvider)
        where TContract : notnull =>
        serviceProvider.GetRequiredService<TContract>();

    private static TImplementation Resolve<TContract, TImplementation>(this IServiceProvider serviceProvider)
        where TContract : notnull
        where TImplementation : TContract =>
        (TImplementation)serviceProvider.GetRequiredService<TContract>();
}