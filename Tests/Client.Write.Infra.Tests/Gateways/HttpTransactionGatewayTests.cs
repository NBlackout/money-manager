using System.Text.Json;
using Client.Write.Infra.Gateways;

namespace Client.Write.Infra.Tests.Gateways;

public class HttpTransactionGatewayTests : InfraTest<ITransactionGateway, HttpTransactionGateway>
{
    private const string ApiUrl = "http://localhost/api";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory]
    [RandomData]
    public async Task Assigns_category(Guid transactionId, Guid categoryId)
    {
        await this.Sut.AssignCategory(transactionId, categoryId);
        this.Verify_Put($"{ApiUrl}/transactions/{transactionId}/category", new { CategoryId = categoryId });
    }

    private void Verify_Put(string url, object payload) =>
        this
            .httpMessageHandler
            .Calls
            .Should()
            .Equal((HttpMethod.Put, url, JsonSerializer.Serialize(payload, Defaults.JsonSerializerOptions)));

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}