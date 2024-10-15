namespace Client.Read.App.Tests.UseCases;

public class CategorizationSuggestionsTests
{
    private readonly StubbedCategorizationGateway gateway = new();
    private readonly CategorizationSuggestions sut;

    public CategorizationSuggestionsTests()
    {
        this.sut = new CategorizationSuggestions(this.gateway);
    }

    [Theory, RandomData]
    public async Task Gives_account_summaries(CategorizationSuggestionPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(CategorizationSuggestionPresentation[] expected)
    {
        CategorizationSuggestionPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(CategorizationSuggestionPresentation[] expected) =>
        this.gateway.Feed(expected);
}