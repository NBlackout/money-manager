using System.Text.Json;
using Client.Write.Infra.Gateways;

namespace Client.Write.Infra.Tests.Gateways;

public class HttpAccountGatewayTests : InfraTest<IAccountGateway, HttpAccountGateway>
{
    private const string ApiUrl = "http://localhost/api";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory]
    [RandomData]
    public async Task Stops_tracking(Guid id)
    {
        await this.Sut.StopTracking(id);
        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = false });
    }

    [Theory]
    [RandomData]
    public async Task Resumes_tracking(Guid id)
    {
        await this.Sut.ResumeTracking(id);
        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = true });
    }

    [Theory]
    [RandomData]
    public async Task Assigns_label(Guid id, string label)
    {
        await this.Sut.AssignLabel(id, label);
        this.Verify_Put($"{ApiUrl}/accounts/{id}/label", new { Label = label });
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