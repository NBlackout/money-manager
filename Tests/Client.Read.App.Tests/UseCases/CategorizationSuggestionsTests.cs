using Client.Read.Infra.Gateways.Categorization;

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
    public async Task Retrieves_account_summaries(CategorizationSuggestionPresentation[] expected)
    {
        this.gateway.Feed(expected);
        CategorizationSuggestionPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}