namespace Client.Read.App.Tests.TestDoubles;

public class StubbedCategorizationGateway : ICategorizationGateway
{
    private CategorizationSuggestionPresentation[] suggestions = null!;

    public Task<CategorizationSuggestionPresentation[]> Suggestions() =>
        Task.FromResult(this.suggestions);

    public void Feed(CategorizationSuggestionPresentation[] expected) =>
        this.suggestions = expected;
}