using Client.Read.Infra.Gateways.Categorization;
using Client.Read.Infra.Tests.TestDoubles;
using Shared.Presentation;

namespace Client.Read.Infra.Tests.Gateways;

public sealed class HttpCategorizationGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();
    private readonly HttpCategorizationGateway sut;

    public HttpCategorizationGatewayTests()
    {
        this.sut = this.Resolve<ICategorizationGateway, HttpCategorizationGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddReadInfra().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Retrieves_categorization_suggestions(CategorizationSuggestionPresentation[] expected)
    {
        this.httpMessageHandler.Feed($"{ApiUrl}/categorization", expected);
        CategorizationSuggestionPresentation[] actual = await this.sut.Suggestions();
        actual.Should().Equal(expected);
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}