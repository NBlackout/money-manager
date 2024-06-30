using System.Text.Json;
using Client.Extensions;
using Client.Write.Infra.Gateways.Budget;
using Client.Write.Infra.Tests.TestDoubles;

namespace Client.Write.Infra.Tests.Gateways;

public sealed class HttpBudgetGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();
    private readonly HttpBudgetGateway sut;

    public HttpBudgetGatewayTests()
    {
        this.sut = this.Resolve<IBudgetGateway, HttpBudgetGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Defines(Guid id, string name, decimal amount)
    {
        await this.sut.Define(id, name, amount);
        this.Verify_Post($"{ApiUrl}/budgets", new { Id = id, Name = name, Amount = amount });
    }

    private void Verify_Post(string url, object payload)
    {
        JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        this.httpMessageHandler.Calls.Should()
            .Equal((HttpMethod.Post, url, JsonSerializer.Serialize(payload, jsonSerializerOptions)));
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}