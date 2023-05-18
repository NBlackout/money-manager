using System.Text.Json;
using MoneyManager.Client.Extensions;
using MoneyManager.Client.Write.Infrastructure.Gateways.Transaction;
using MoneyManager.Client.Write.Infrastructure.Tests.TestDoubles;

namespace MoneyManager.Client.Write.Infrastructure.Tests.Gateways;

public sealed class HttpTransactionGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpTransactionGateway sut;

    public HttpTransactionGatewayTests()
    {
        this.httpMessageHandler = new SpyHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.sut = this.host.Service<ITransactionGateway, HttpTransactionGateway>();
    }

    [Fact]
    public async Task Should_assign_category()
    {
        Guid transactionId = Guid.NewGuid();
        Guid categoryId = Guid.NewGuid();

        await this.sut.AssignCategory(transactionId, categoryId);

        this.Verify_Put($"{ApiUrl}/transactions/{transactionId}/category", new { CategoryId = categoryId });
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