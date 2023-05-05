using System.Net.Http.Json;

namespace MoneyManager.Client.Read.Infrastructure.Gateways.AccountSummaries;

public class HttpAccountSummariesGateway : IAccountSummariesGateway
{
    private readonly HttpClient httpClient;

    public HttpAccountSummariesGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<AccountSummaryPresentation>> Get() =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummaryPresentation>>("accounts"))!;
}