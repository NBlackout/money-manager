using MoneyManager.Client.Read.Infrastructure.Gateways.Category;
using MoneyManager.Client.Read.Infrastructure.Tests.TestDoubles;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Client.Read.Infrastructure.Tests.Gateways;

public sealed class HttpCategoryGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpCategoryGateway sut;

    public HttpCategoryGatewayTests()
    {
        this.httpMessageHandler = new StubbedHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddReadDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.sut = this.host.Service<ICategoryGateway, HttpCategoryGateway>();
    }

    [Fact]
    public async Task Should_retrieve_category_summaries()
    {
        CategorySummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), "Category"),
            new(Guid.NewGuid(), "A category")
        };
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/categories", expected);

        IReadOnlyCollection<CategorySummaryPresentation> actual = await this.sut.Summaries();
        actual.Should().Equal(expected);
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}