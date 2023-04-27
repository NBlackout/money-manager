using System.Net.Http.Json;
using MoneyManager.Shared;

namespace MoneyManager.Client.Read.Infrastructure.AccountSummariesGateway;

public class HttpAccountSummariesGateway : IAccountSummariesGateway
{
    private readonly HttpClient httpClient;

    public HttpAccountSummariesGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<AccountSummary>> Get() =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<AccountSummary>>("accounts"))!;
}