using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Client.Extensions;

public static class WebAssemblyHostBuilderExtensions
{
    public static WebAssemblyHostBuilder AddServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }).AddWrite().AddRead();

        return builder;
    }
}