namespace Client.Read.Infra.Gateways.Categorization;

public class HttpCategorizationGateway : ICategorizationGateway
{
    private readonly HttpClient httpClient;

    public HttpCategorizationGateway(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        (await this.httpClient.GetFromJsonAsync<CategorizationSuggestionPresentation[]>("categorization"))!;
}