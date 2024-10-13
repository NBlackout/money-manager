using Client.Read.Infra;
using Client.Write.Infra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Read.Infra;
using Shared.TestTooling;
using Write.Infra;

namespace Shared.Infra.TestTooling;

public abstract class HostFixture : IDisposable
{
    private readonly IHost host;
    private readonly IServiceScope scope;

    protected HostFixture()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(s =>
            {
                s
                    .AddSharedInfra()
                    .AddScoped(_ => new HttpClient())
                    .AddServerWriteInfra()
                    .AddServerReadInfra()
                    .AddClientWriteInfra()
                    .AddClientReadInfra();

                this.Configure(s);
            })
            .UseEnvironment("Development")
            .Build();

        this.scope = this.host.Services.CreateScope();
    }


    protected virtual void Configure(IServiceCollection services)
    {
    }

    protected TImplementation Resolve<TContract, TImplementation>()
        where TContract : notnull
        where TImplementation : TContract =>
        this.scope.Resolve<TContract, TImplementation>();

    protected TContract Resolve<TContract>() where TContract : notnull =>
        this.scope.Resolve<TContract>();

    protected TOptions ResolveOptions<TOptions>() where TOptions : class =>
        this.host.ResolveOptions<TOptions>();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        this.scope.Dispose();
        this.host.Dispose();
    }
}