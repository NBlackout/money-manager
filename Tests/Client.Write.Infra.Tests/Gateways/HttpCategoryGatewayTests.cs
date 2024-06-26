using System.Text.Json;
using Client.Extensions;
using Client.Write.Infra.Gateways.Category;
using Client.Write.Infra.Tests.TestDoubles;

namespace Client.Write.Infra.Tests.Gateways;

public sealed class HttpCategoryGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();
    private readonly HttpCategoryGateway sut;

    public HttpCategoryGatewayTests()
    {
        this.sut = this.Resolve<ICategoryGateway, HttpCategoryGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Should_create(Guid id, string label, string keywords)
    {
        await this.sut.Create(id, label, keywords);
        this.Verify_Post($"{ApiUrl}/categories", new { Id = id, Label = label, Keywords = keywords });
    }

    [Theory, RandomData]
    public async Task Should_delete(Guid id)
    {
        await this.sut.Delete(id);
        this.Verify_Delete($"{ApiUrl}/categories/{id}");
    }

    private void Verify_Post(string url, object payload)
    {
        JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        this.httpMessageHandler.Calls.Should()
            .Equal((HttpMethod.Post, url, JsonSerializer.Serialize(payload, jsonSerializerOptions)));
    }

    private void Verify_Delete(string url) => 
        this.httpMessageHandler.Calls.Should().Equal((HttpMethod.Delete, url, string.Empty));

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}