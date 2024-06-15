namespace Client.Read.Infra.Gateways.Account;

public class HttpAccountGateway : IAccountGateway
{
    private readonly HttpClient httpClient;

    public HttpAccountGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<AccountSummaryPresentation[]> Summaries() =>
        (await this.httpClient.GetFromJsonAsync<AccountSummaryPresentation[]>("accounts"))!;

    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        (await this.httpClient.GetFromJsonAsync<AccountDetailsPresentation>($"accounts/{id}"))!;

    public async Task<TransactionSummaryPresentation[]> TransactionsOfMonth(Guid id, int year, int month) =>
        (await this.httpClient.GetFromJsonAsync<TransactionSummaryPresentation[]>(
            $"accounts/{id}/transactions?year={year}&month={month}"))!;
}