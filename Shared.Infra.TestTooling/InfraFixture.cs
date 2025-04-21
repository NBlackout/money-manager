using Client.Read.Infra;
using Client.Write.Infra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Read.Infra;
using Shared.TestTooling;
using Write.Infra;

namespace Shared.Infra.TestTooling;

public abstract class InfraFixture<TContract> : InfraFixture<TContract, TContract> where TContract : notnull;

public abstract class InfraFixture<TContract, TImplementation> : InfraFixture
    where TContract : notnull where TImplementation : TContract
{
    protected readonly TImplementation Sut;

    protected InfraFixture() =>
        this.Sut = this.Resolve<TContract, TImplementation>();
}

public abstract class InfraFixture : IDisposable
{
    private readonly IHost host;
    private readonly IServiceScope scope;

    protected InfraFixture()
    {
        this.host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(
                s =>
                {
                    s
                        .AddSharedInfra()
                        .AddScoped(_ => new HttpClient())
                        .AddServerWriteInfra()
                        .AddServerReadInfra()
                        .AddClientWriteInfra()
                        .AddClientReadInfra();

                    this.Configure(s);
                }
            )
            .UseEnvironment("Development")
            .Build();

        this.scope = this.host.Services.CreateScope();
    }

    protected virtual void Configure(IServiceCollection services)
    {
    }

    protected TImplementation Resolve<TContract, TImplementation>()
        where TContract : notnull where TImplementation : TContract =>
        this.scope.Resolve<TContract, TImplementation>();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        this.scope.Dispose();
        this.host.Dispose();
    }
}