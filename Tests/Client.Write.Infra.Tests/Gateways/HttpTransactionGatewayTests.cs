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

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Assigns_category(Guid transactionId, Guid categoryId)
    {
        await this.sut.AssignCategory(transactionId, categoryId);
        this.Verify_Put($"{ApiUrl}/transactions/{transactionId}/category", new { CategoryId = categoryId });
    }

    private void Verify_Put(string url, object payload)
    {
        this.httpMessageHandler.Calls.Should().Equal((HttpMethod.Put, url,
            JsonSerializer.Serialize(payload, Defaults.JsonSerializerOptions)));
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}