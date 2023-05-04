using System.Text.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Write.Infrastructure.AccountGateway;
using MoneyManager.Client.Write.Infrastructure.Tests.TestDoubles;

namespace MoneyManager.Client.Write.Infrastructure.Tests;

public sealed class HttpAccountGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpAccountGateway sut;

    public HttpAccountGatewayTests()
    {
        this.httpMessageHandler = new SpyHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.sut = this.host.GetRequiredService<IAccountGateway, HttpAccountGateway>();
    }

    [Fact]
    public async Task Should_stop_tracking()
    {
        Guid id = Guid.NewGuid();

        await this.sut.StopTracking(id);

        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = false });
    }

    [Fact]
    public async Task Should_resume_tracking()
    {
        Guid id = Guid.NewGuid();

        await this.sut.ResumeTracking(id);

        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = true });
    }


    [Fact]
    public async Task Should_assign_label()
    {
        Guid id = Guid.NewGuid();
        const string label = "My account label";

        await this.sut.AssignLabel(id, label);

        this.Verify_Put($"{ApiUrl}/accounts/{id}/label", new { Label = label });
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