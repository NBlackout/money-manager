namespace Client.Read.Infra.Tests.Gateways;

public sealed class HttpCategorizationGatewayTests : InfraFixture<ICategorizationGateway, HttpCategorizationGateway>
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Gives_categorization_suggestions(CategorizationSuggestionPresentation[] expected)
    {
        this.Feed("categorization", expected);
        CategorizationSuggestionPresentation[] actual = await this.Sut.Suggestions();
        actual.Should().Equal(expected);
    }

    private void Feed(object url, object expected) =>
        this.httpMessageHandler.Feed($"{ApiUrl}/{url}", expected);

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}