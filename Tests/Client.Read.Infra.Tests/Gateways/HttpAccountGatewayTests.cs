using Client.Read.Infra.Gateways.Account;
using Client.Read.Infra.Tests.TestDoubles;
using Shared.Presentation;

namespace Client.Read.Infra.Tests.Gateways;

public sealed class HttpAccountGatewayTests : HostFixture
{
    private const string ApiUrl = "http://localhost";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();
    private readonly HttpAccountGateway sut;

    public HttpAccountGatewayTests()
    {
        this.sut = this.Resolve<IAccountGateway, HttpAccountGateway>();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddReadDependencies().AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "Account A", "Numb1", 351.30m, DateTime.Today, false),
            new(Guid.NewGuid(), Guid.NewGuid(), "Account B", "Numb2", 8901.04m,
                DateTime.Today.AddHours(-4), true)
        };
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts", expected);

        IReadOnlyCollection<AccountSummaryPresentation> actual = await this.sut.Summaries();
        actual.Should().Equal(expected);
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountDetailsPresentation expected = new(Guid.NewGuid(), "Some account", "ABC123", 185.46m,
            DateTime.Parse("2413-03-30"));
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts/{expected.Id}", expected);

        AccountDetailsPresentation actual = await this.sut.Details(expected.Id);
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
            new(Guid.NewGuid(), 1.99m, "Credit", DateTime.Parse("2014-12-24"), "Healthcare"),
            new(Guid.NewGuid(), 1.99m, "Credit", DateTime.Parse("2000-08-11"), null)
        };
        this.httpMessageHandler.SetResponseFor($"{ApiUrl}/accounts/{accountId}/transactions?year={year}&month={month}",
            expected);

        IReadOnlyCollection<TransactionSummaryPresentation> actual =
            await this.sut.TransactionsOfMonth(accountId, year, month);
        actual.Should().Equal(expected);
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}