namespace Client.Read.Infra.Gateways;

public class HttpCategorizationGateway(HttpClient httpClient) : ICategorizationGateway
{
    public async Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        (await httpClient.GetFromJsonAsync<CategorizationSuggestionPresentation[]>("api/categorization"))!;
}