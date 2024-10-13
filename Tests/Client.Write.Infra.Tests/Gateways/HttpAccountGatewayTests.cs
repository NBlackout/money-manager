using System.Text.Json;
using Client.Extensions;
using Client.Write.Infra.Gateways.Account;
using Client.Write.Infra.Tests.TestDoubles;

namespace Client.Write.Infra.Tests.Gateways;

public sealed class HttpAccountGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly SpyHttpMessageHandler httpMessageHandler = new();
    private readonly HttpAccountGateway sut;

    public HttpAccountGatewayTests()
    {
        this.sut = this.Resolve<IAccountGateway, HttpAccountGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Stops_tracking(Guid id)
    {
        await this.sut.StopTracking(id);
        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = false });
    }

    [Theory, RandomData]
    public async Task Resumes_tracking(Guid id)
    {
        await this.sut.ResumeTracking(id);
        this.Verify_Put($"{ApiUrl}/accounts/{id}/tracking", new { Enabled = true });
    }


    [Theory, RandomData]
    public async Task Assigns_label(Guid id, string label)
    {
        await this.sut.AssignLabel(id, label);
        this.Verify_Put($"{ApiUrl}/accounts/{id}/label", new { Label = label });
    }

    private void Verify_Put(string url, object payload)
    {
        this.httpMessageHandler.Calls.Should().Equal((HttpMethod.Put, url,
            JsonSerializer.Serialize(payload, Defaults.JsonSerializerOptions)));
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpResponseMessage) =>
        new(httpResponseMessage) { BaseAddress = new Uri(ApiUrl) };
}