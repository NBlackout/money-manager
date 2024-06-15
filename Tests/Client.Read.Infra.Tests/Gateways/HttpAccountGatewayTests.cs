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

    [Theory, RandomData]
    public async Task Should_retrieve_account_summaries(AccountSummaryPresentation[] expected)
    {
        this.httpMessageHandler.Feed($"{ApiUrl}/accounts", expected);
        AccountSummaryPresentation[] actual = await this.sut.Summaries();
        actual.Should().Equal(expected);
    }

    [Theory, RandomData]
    public async Task Should_retrieve_account_details(AccountDetailsPresentation expected)
    {
        this.httpMessageHandler.Feed($"{ApiUrl}/accounts/{expected.Id}", expected);
        AccountDetailsPresentation actual = await this.sut.Details(expected.Id);
        actual.Should().Be(expected);
    }

    [Theory, RandomData]
    public async Task Should_retrieve_transactions_of_month(
        Guid accountId,
        int year,
        int month,
        TransactionSummaryPresentation[] expected)
    {
        this.httpMessageHandler.Feed($"{ApiUrl}/accounts/{accountId}/transactions?year={year}&month={month}", expected);
        TransactionSummaryPresentation[] actual = await this.sut.TransactionsOfMonth(accountId, year, month);
        actual.Should().Equal(expected);
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}