namespace Client.Read.Infra.Gateways;

public class HttpAccountGateway(HttpClient httpClient) : IAccountGateway
{
    public async Task<AccountSummaryPresentation[]> Summaries() =>
        (await httpClient.GetFromJsonAsync<AccountSummaryPresentation[]>("api/accounts"))!;

    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        (await httpClient.GetFromJsonAsync<AccountDetailsPresentation>($"api/accounts/{id}"))!;

    public async Task<TransactionSummaryPresentation[]> TransactionsOfMonth(Guid id, int year, int month) =>
        (await httpClient.GetFromJsonAsync<TransactionSummaryPresentation[]>(
            $"api/accounts/{id}/transactions?year={year}&month={month}"
        ))!;
}