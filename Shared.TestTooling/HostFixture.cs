﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.TestTooling;

public abstract class HostFixture : IDisposable
{
    private readonly IHost host;
    private readonly IServiceScope scope;

    protected HostFixture()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(this.Configure)
            .UseEnvironment("Development")
            .Build();

        this.scope = this.host.Services.CreateScope();
    }


    protected virtual void Configure(IServiceCollection services)
    {
    }

    public TImplementation Resolve<TContract, TImplementation>()
        where TContract : notnull
        where TImplementation : TContract =>
        this.scope.Resolve<TContract, TImplementation>();

    public TContract Resolve<TContract>() where TContract : notnull =>
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