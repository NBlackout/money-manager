using MoneyManager.Client.Read.Infrastructure.AccountSummariesGateway;
using MoneyManager.Client.Read.Infrastructure.Tests.TestDoubles;

namespace MoneyManager.Client.Read.Infrastructure.Tests;

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
        AccountSummary[] expected =
        {
            new(Guid.NewGuid(), "Account A", 351.30m, false),
            new(Guid.NewGuid(), "Account B", 8901.04m, true)
        };
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts", expected);

        IReadOnlyCollection<AccountSummary> actual = await this.sut.Get();
        actual.Should().Contain(expected);
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}