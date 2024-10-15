namespace Client.Read.Infra.Gateways;

public class HttpAccountGateway(HttpClient httpClient) : IAccountGateway
{
    public async Task<AccountSummaryPresentation[]> Summaries() =>
        (await httpClient.GetFromJsonAsync<AccountSummaryPresentation[]>("accounts"))!;

    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        (await httpClient.GetFromJsonAsync<AccountDetailsPresentation>($"accounts/{id}"))!;

    public async Task<TransactionSummaryPresentation[]> TransactionsOfMonth(Guid id, int year, int month) =>
        (await httpClient.GetFromJsonAsync<TransactionSummaryPresentation[]>(
            $"accounts/{id}/transactions?year={year}&month={month}"))!;
}
