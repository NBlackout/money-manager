namespace Client.Read.Infra.Tests.Gateways;

public class HttpCategoryGatewayTests : InfraTest<ICategoryGateway, HttpCategoryGateway>
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Gives_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.Feed("categories", expected);
        CategorySummaryPresentation[] actual = await this.Sut.Summaries();
        actual.Should().Equal(expected);
    }

    private void Feed(object url, object expected) =>
        this.httpMessageHandler.Feed($"{ApiUrl}/{url}", expected);

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}