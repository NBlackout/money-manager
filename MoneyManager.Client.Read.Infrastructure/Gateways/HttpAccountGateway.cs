namespace MoneyManager.Client.Read.Infrastructure.Gateways;

public class HttpAccountGateway : IAccountSummariesGateway, IAccountDetailsGateway, ITransactionsOfMonthGateway
{
    private readonly HttpClient httpClient;

    public HttpAccountGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Get() =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummaryPresentation>>("accounts"))!;

    public async Task<AccountDetailsPresentation> Get(Guid id) =>
        (await this.httpClient.GetFromJsonAsync<AccountDetailsPresentation>($"accounts/{id}"))!;

    public async Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid id, int year, int month) =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<TransactionSummaryPresentation>>(
            $"accounts/{id}/transactions?year={year}&month={month}"))!;
}