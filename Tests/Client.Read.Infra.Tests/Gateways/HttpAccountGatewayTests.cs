namespace Client.Read.Infra.Tests.Gateways;

public class HttpAccountGatewayTests : InfraTest<IAccountGateway, HttpAccountGateway>
{
    private const string ApiUrl = "http://localhost/api";

    private readonly StubbedHttpMessageHandler httpMessageHandler = new();

    protected override void Configure(IServiceCollection services) =>
        services.AddScoped(_ => CreateHttpClient(this.httpMessageHandler));

    [Theory]
    [RandomData]
    public async Task Gives_account_summaries(AccountSummaryPresentation[] expected)
    {
        this.Feed("accounts", expected);
        AccountSummaryPresentation[] actual = await this.Sut.Summaries();
        actual.Should().Equal(expected);
    }

    [Theory]
    [RandomData]
    public async Task Gives_account_details(AccountDetailsPresentation expected)
    {
        this.Feed($"accounts/{expected.Id}", expected);
        AccountDetailsPresentation actual = await this.Sut.Details(expected.Id);
        actual.Should().Be(expected);
    }

    [Theory]
    [RandomData]
    public async Task Gives_transactions_of_month(
        Guid accountId,
        int year,
        int month,
        TransactionSummaryPresentation[] expected)
    {
        this.Feed($"accounts/{accountId}/transactions?year={year}&month={month}", expected);
        TransactionSummaryPresentation[] actual = await this.Sut.TransactionsOfMonth(accountId, year, month);
        actual.Should().Equal(expected);
    }

    private void Feed(string url, object expected) =>
        this.httpMessageHandler.Feed($"{ApiUrl}/{url}", expected);

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) =>
        new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };
}