using System.Text.Json;
using Client.Write.Infra.Gateways;

namespace Client.Write.Infra.Tests.Gateways;

public class HttpCategoryGatewayTests : InfraTest<ICategoryGateway, HttpCategoryGateway>
{
    private const string ApiUrl = "http://localhost/api";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory]
    [RandomData]
    public async Task Creates(Guid id, string label, string keywords)
    {
        await this.Sut.Create(id, label, keywords);
        this.Verify_Post($"{ApiUrl}/categories", new { Id = id, Label = label, Keywords = keywords });
    }

    [Theory]
    [RandomData]
    public async Task Deletes(Guid id)
    {
        await this.Sut.Delete(id);
        this.Verify_Delete($"{ApiUrl}/categories/{id}");
    }

    private void Verify_Post(string url, object payload) =>
        this
            .httpMessageHandler
            .Calls
            .Should()
            .Equal((HttpMethod.Post, url, JsonSerializer.Serialize(payload, Defaults.JsonSerializerOptions)));

    private void Verify_Delete(string url) =>
        this.httpMessageHandler.Calls.Should().Equal((HttpMethod.Delete, url, string.Empty));

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}