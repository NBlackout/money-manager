using Client.Read.Infra.Gateways.Category;
using Client.Read.Infra.Tests.TestDoubles;
using Shared.Infra.TestTooling;
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
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory, RandomData]
    public async Task Retrieves_category_summaries(CategorySummaryPresentation[] expected)
    {
        this.Feed("categories", expected);
        CategorySummaryPresentation[] actual = await this.sut.Summaries();
        actual.Should().Equal(expected);
    }

    private void Feed(object url, object expected) =>
        this.httpMessageHandler.Feed($"{ApiUrl}/{url}", expected);

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}