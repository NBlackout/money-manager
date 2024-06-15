using System.Text.Json;
using Client.Extensions;
using Client.Write.Infra.Gateways.Transaction;
using Client.Write.Infra.Tests.TestDoubles;

namespace Client.Write.Infra.Tests.Gateways;

public sealed class HttpTransactionGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();
    private readonly HttpTransactionGateway sut;

    public HttpTransactionGatewayTests()
    {
        this.sut = this.Resolve<ITransactionGateway, HttpTransactionGateway>();
    }

    protected override void Configure(IServiceCollection services) => services.AddWriteDependencies()
        .AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Fact]
    public async Task Should_assign_category()
    {
        Guid transactionId = Guid.NewGuid();
        Guid categoryId = Guid.NewGuid();

        await this.sut.AssignCategory(transactionId, categoryId);

        this.Verify_Put($"{ApiUrl}/transactions/{transactionId}/category", new { CategoryId = categoryId });
    }

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