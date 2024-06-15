namespace Client.Read.Infra.Gateways.Category;

public class HttpCategoryGateway : ICategoryGateway
{
    private readonly HttpClient httpClient;

    public HttpCategoryGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CategorySummaryPresentation[]> Summaries() =>
        (await this.httpClient.GetFromJsonAsync<CategorySummaryPresentation[]>("categories"))!;
}