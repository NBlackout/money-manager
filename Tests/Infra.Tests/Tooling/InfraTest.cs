using Infra.Read;
using Infra.Shared;
using Infra.Write;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infra.Tests.Tooling;

public abstract class InfraTest<TContract> : InfraTest<TContract, TContract> where TContract : notnull;

public abstract class InfraTest<TContract, TImplementation> : InfraTest where TContract : notnull where TImplementation : TContract
{
    protected readonly TImplementation Sut;

    protected InfraTest()
    {
        this.Sut = this.Resolve<TContract, TImplementation>();
    }
}

public abstract class InfraTest : IDisposable
{
    private readonly IHost host;
    private readonly IServiceScope scope;

    protected InfraTest()
    {
        this.host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(s =>
                {
                    s.AddSharedInfra().AddScoped(_ => new HttpClient()).AddWriteInfra().AddReadInfra();

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

    protected TImplementation Resolve<TContract, TImplementation>() where TContract : notnull where TImplementation : TContract =>
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