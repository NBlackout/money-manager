using Client.Read.Infra.Gateways.Category;
using Client.Read.Infra.Tests.TestDoubles;
using Shared.Presentation;

namespace Client.Read.Infra.Tests.Gateways;

public sealed class HttpCategoryGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();
    private readonly HttpCategoryGateway sut;

    public HttpCategoryGatewayTests()
    {
        this.sut = this.Resolve<ICategoryGateway, HttpCategoryGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddReadDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Should_retrieve_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.httpMessageHandler.Feed($"{ApiUrl}/categories", expected);
        CategorySummaryPresentation[] actual = await this.sut.Summaries();
        actual.Should().Equal(expected);
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}