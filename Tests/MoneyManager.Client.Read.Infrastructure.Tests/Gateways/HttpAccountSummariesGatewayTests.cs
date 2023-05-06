using MoneyManager.Client.Read.Infrastructure.Gateways.AccountSummaries;
using MoneyManager.Client.Read.Infrastructure.Tests.TestDoubles;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Client.Read.Infrastructure.Tests.Gateways;

public sealed class HttpAccountSummariesGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpAccountSummariesGateway sut;

    public HttpAccountSummariesGatewayTests()
    {
        this.httpMessageHandler = new StubbedHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddReadDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.sut = this.host.GetRequiredService<IAccountSummariesGateway, HttpAccountSummariesGateway>();
    }

    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "Bank", "Account A", 351.30m, DateTime.Today, false),
            new(Guid.NewGuid(), Guid.NewGuid(), "Other bank", "Account B", 8901.04m, DateTime.Today.AddHours(-4), true)
        };
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts", expected);

        IReadOnlyCollection<AccountSummaryPresentation> actual = await this.sut.Get();
        actual.Should().Contain(expected);
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}