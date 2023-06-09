﻿namespace MoneyManager.Client.Read.Infrastructure.Gateways.Account;

public class HttpAccountGateway : IAccountGateway
{
    private readonly HttpClient httpClient;

    public HttpAccountGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Summaries() =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummaryPresentation>>("accounts"))!;

    public async Task<AccountDetailsPresentation> Details(Guid id) =>
        (await this.httpClient.GetFromJsonAsync<AccountDetailsPresentation>($"accounts/{id}"))!;

    public async Task<IReadOnlyCollection<TransactionSummaryPresentation>> TransactionsOfMonth(Guid id, int year, int month) =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<TransactionSummaryPresentation>>(
            $"accounts/{id}/transactions?year={year}&month={month}"))!;
}