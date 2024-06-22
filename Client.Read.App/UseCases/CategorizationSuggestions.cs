namespace Client.Read.App.UseCases;

public class CategorizationSuggestions
{
    private readonly ICategorizationGateway gateway;

    public CategorizationSuggestions(ICategorizationGateway gateway)
    {
        this.gateway = gateway;
    }

    public async Task<CategorizationSuggestionPresentation[]> Execute() =>
        await this.gateway.Suggestions();
}