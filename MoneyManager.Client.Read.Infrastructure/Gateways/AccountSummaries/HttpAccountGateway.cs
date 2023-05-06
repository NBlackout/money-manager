namespace MoneyManager.Client.Read.Infrastructure.Gateways.AccountSummaries;

public class HttpAccountGateway : IAccountSummariesGateway, IAccountDetailsGateway
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
}