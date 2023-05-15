namespace MoneyManager.Client.Read.Infrastructure.Gateways.Category;

public class HttpCategoryGateway : ICategoryGateway
{
    private readonly HttpClient httpClient;

    public HttpCategoryGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CategorySummaryPresentation>> Summaries() =>
        (await this.httpClient.GetFromJsonAsync<IReadOnlyCollection<CategorySummaryPresentation>>("categories"))!;
}