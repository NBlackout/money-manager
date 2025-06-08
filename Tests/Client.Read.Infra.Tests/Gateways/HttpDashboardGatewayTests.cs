namespace Client.Read.Infra.Tests.Gateways;

public class HttpDashboardGatewayTests : InfraTest<IDashboardGateway, HttpDashboardGateway>
{
    private const string ApiUrl = "http://localhost/api";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory]
    [RandomData]
    public async Task Gives_sliding_account_balances(SlidingAccountBalancesPresentation expected)
    {
        this.Feed("dashboard/sliding-account-balances", expected);
        SlidingAccountBalancesPresentation actual = await this.Sut.SlidingAccountBalances();
        actual.Should().BeEquivalentTo(expected);
    }

    private void Feed(object url, object expected) =>
        this.httpMessageHandler.Feed($"{ApiUrl}/{url}", expected);

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}