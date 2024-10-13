namespace Client.Read.Infra.Gateways.Categorization;

public class StubbedCategorizationGateway : ICategorizationGateway
{
    private CategorizationSuggestionPresentation[] suggestions = null!;

    public Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        Task.FromResult(this.suggestions);

    public void Feed(CategorizationSuggestionPresentation[] expected) =>
        this.suggestions = expected;
}