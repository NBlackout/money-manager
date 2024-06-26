namespace Client.Read.App.UseCases;

public class CategorizationSuggestions(ICategorizationGateway gateway)
{
    public async Task<CategorizationSuggestionPresentation[]> Execute() =>
        await gateway.Suggestions();
}