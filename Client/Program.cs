using Client.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Client.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.AddServices().Build().RunAsync();