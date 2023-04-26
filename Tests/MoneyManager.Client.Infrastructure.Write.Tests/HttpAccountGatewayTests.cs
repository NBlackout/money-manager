using System.Text.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Infrastructure.Write.AccountGateway;
using MoneyManager.Client.Infrastructure.Write.Tests.TestDoubles;

namespace MoneyManager.Client.Infrastructure.Write.Tests;

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
    public async Task Should_stop_tracking_account()
    {
        Guid id = Guid.Parse("1A87A411-BBEB-4FB0-83E7-539CF5EFBE6C");

        await this.sut.StopTracking(id);

        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = false });
    }

    [Fact]
    public async Task Should_resume_account_tracking()
    {
        Guid id = Guid.Parse("2981760C-A17B-4ECF-B828-AB89CCD1B11A");

        await this.sut.ResumeTracking(id);

        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = true });
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