using System.Text.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Write.Infrastructure.Gateways.Category;
using MoneyManager.Client.Write.Infrastructure.Tests.TestDoubles;

namespace MoneyManager.Client.Write.Infrastructure.Tests.Gateways;

public sealed class HttpCategoryGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpCategoryGateway sut;

    public HttpCategoryGatewayTests()
    {
        this.httpMessageHandler = new SpyHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.sut = this.host.Service<ICategoryGateway, HttpCategoryGateway>();
    }

    [Fact]
    public async Task Should_assign_label()
    {
        Guid id = Guid.NewGuid();
        const string label = "My category label";

        await this.sut.Create(id, label);

        this.Verify_Post($"{ApiUrl}/categories", new { Id = id, Label = label });
    }

    public void Dispose() =>
        this.host.Dispose();

    private void Verify_Post(string url, object payload)
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