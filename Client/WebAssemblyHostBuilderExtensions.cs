using Client.Read.Infra;
using Client.Write.Infra;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Client;

public static class WebAssemblyHostBuilderExtensions
{
    public static void AddServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services
            .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/") })
            .AddClientWriteInfra()
            .AddClientReadInfra();
    }
}