namespace Client.Read.Infra.Gateways;

public class HttpCategoryGateway(HttpClient httpClient) : ICategoryGateway
{
    public async Task<CategorySummaryPresentation[]> Summaries() =>
        (await httpClient.GetFromJsonAsync<CategorySummaryPresentation[]>("api/categories"))!;
}