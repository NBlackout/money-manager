namespace Client.Read.Infra.Gateways.Categorization;

public class HttpCategorizationGateway(HttpClient httpClient) : ICategorizationGateway
{
    public async Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        (await httpClient.GetFromJsonAsync<CategorizationSuggestionPresentation[]>("categorization"))!;
}