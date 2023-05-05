using System.Text.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Write.Infrastructure.BankGateway;
using MoneyManager.Client.Write.Infrastructure.Tests.TestDoubles;

namespace MoneyManager.Client.Write.Infrastructure.Tests;

public sealed class HttpBankGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpBankGateway sut;

    public HttpBankGatewayTests()
    {
        this.httpMessageHandler = new SpyHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.sut = this.host.GetRequiredService<IBankGateway, HttpBankGateway>();
    }

    [Fact]
    public async Task Should_assign_name()
    {
        Guid id = Guid.NewGuid();
        const string name = "The bank name";

        await this.sut.AssignName(id, name);

        this.Verify_Put($"{ApiUrl}/banks/{id}/name", new { Name = name });
    }

    public void Dispose() =>
        this.host.Dispose();

    private void Verify_Put(string url, object payload)
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        this.httpMessageHandler.Calls.Should().Equal((url, JsonSerializer.Serialize(payload, jsonSerializerOptions)));
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}