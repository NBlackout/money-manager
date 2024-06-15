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

    [Fact]
    public async Task Should_assign_label()
    {
        Guid id = Guid.NewGuid();
        const string label = "My category label";

        await this.sut.Create(id, label);

        this.Verify_Post($"{ApiUrl}/categories", new { Id = id, Label = label });
    }

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