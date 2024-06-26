namespace Client.Read.Infra.Gateways.Category;

public class HttpCategoryGateway(HttpClient httpClient) : ICategoryGateway
{
    public async Task<CategorySummaryPresentation[]> Summaries() =>
        (await httpClient.GetFromJsonAsync<CategorySummaryPresentation[]>("categories"))!;
}