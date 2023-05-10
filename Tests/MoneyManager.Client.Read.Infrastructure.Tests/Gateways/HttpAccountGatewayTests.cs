using MoneyManager.Client.Read.Infrastructure.Gateways;
using MoneyManager.Client.Read.Infrastructure.Tests.TestDoubles;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Client.Read.Infrastructure.Tests.Gateways;

public sealed class HttpAccountGatewayTests : IDisposable
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler;
    private readonly IHost host;
    private readonly HttpAccountGateway summariesSut;
    private readonly HttpAccountGateway detailsSut;
    private readonly HttpAccountGateway transactionsOfMonthSut;

    public HttpAccountGatewayTests()
    {
        this.httpMessageHandler = new StubbedHttpMessageHandler();
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.AddReadDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler)))
            .Build();
        this.summariesSut = this.host.Service<IAccountSummariesGateway, HttpAccountGateway>();
        this.detailsSut = this.host.Service<IAccountDetailsGateway, HttpAccountGateway>();
        this.transactionsOfMonthSut = this.host.Service<ITransactionsOfMonthGateway, HttpAccountGateway>();
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

        IReadOnlyCollection<AccountSummaryPresentation> actual = await this.summariesSut.Get();
        actual.Should().Equal(expected);
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountDetailsPresentation expected = new(Guid.NewGuid(), "Some account", "ABC123", 185.46m,
            DateTime.Parse("2413-03-30"));
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts/{expected.Id}", expected);

        AccountDetailsPresentation actual = await this.detailsSut.Get(expected.Id);
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task Should_retrieve_transactions_of_month()
    {
        Guid accountId = Guid.NewGuid();
        const int year = 1999;
        const int month = 8;
        TransactionSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), 1.99m, "Credit", DateTime.Parse("2014-12-24")),
            new(Guid.NewGuid(), 1.99m, "Credit", DateTime.Parse("2000-08-11"))
        };
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts/{accountId}/transactions?year={year}&month={month}",
            expected);

        IReadOnlyCollection<TransactionSummaryPresentation>
            actual = await this.transactionsOfMonthSut.Get(accountId, year, month);
        actual.Should().Equal(expected);
    }

    public void Dispose() =>
        this.host.Dispose();

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}