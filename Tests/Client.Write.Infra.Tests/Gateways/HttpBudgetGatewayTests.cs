using System.Text.Json;
using Client.Write.Infra.Gateways;

namespace Client.Write.Infra.Tests.Gateways;

public class HttpBudgetGatewayTests : InfraTest<IBudgetGateway, HttpBudgetGateway>
{
    private const string ApiUrl = "http://localhost/api";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory]
    [RandomData]
    public async Task Defines(Guid id, string name, decimal amount, DateOnly beginDate)
    {
        await this.Sut.Define(id, name, amount, beginDate);
        this.Verify_Post($"{ApiUrl}/budgets", new { Id = id, Name = name, Amount = amount, BeginDate = beginDate });
    }

    private void Verify_Post(string url, object payload) =>
        this
            .httpMessageHandler
            .Calls
            .Should()
            .Equal((HttpMethod.Post, url, JsonSerializer.Serialize(payload, Defaults.JsonSerializerOptions)));

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}