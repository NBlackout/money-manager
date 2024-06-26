using Read.App.Ports;
using Read.App.UseCases;
using Read.Infra.DataSources.CategoriesWithKeywords;
using Read.Infra.DataSources.TransactionsToCategorize;
using Shared.Presentation;
using static Shared.TestTooling.Randomizer;

namespace Read.App.Tests.UseCases;

public class CategorizationSuggestionsTests
{
    private readonly StubbedCategoriesWithKeywordsDataSource categoriesDataSource = new();
    private readonly StubbedTransactionsToCategorizeDataSource transactionsToCategorizeDataSource = new();
    private readonly CategorizationSuggestions sut;

    public CategorizationSuggestionsTests()
    {
        this.sut = new CategorizationSuggestions(this.categoriesDataSource, this.transactionsToCategorizeDataSource);
    }

    [Fact]
    public async Task Should_give_suggestion_on_exact_match()
    {
        CategoryWithKeywords insuranceCategory = ACategoryMatching("Insurance");
        CategoryWithKeywords travelCategory = ACategoryMatching("Travel");
        this.categoriesDataSource.Feed(insuranceCategory, travelCategory);

        TransactionToCategorize insuranceTransaction = ATransactionLabeled("Insurance");
        TransactionToCategorize travelTransaction = ATransactionLabeled("Travel");
        this.transactionsToCategorizeDataSource.Feed(insuranceTransaction, travelTransaction);

        await this.Verify(
            HasSuggestion(insuranceTransaction, insuranceCategory),
            HasSuggestion(travelTransaction, travelCategory)
        );
    }

    [Fact]
    public async Task Should_give_suggestion_on_partial_match()
    {
        CategoryWithKeywords category = ACategoryMatching("taxi");
        this.categoriesDataSource.Feed(category);

        TransactionToCategorize transaction = ATransactionLabeled("A TAXI fare");
        this.transactionsToCategorizeDataSource.Feed(transaction);

        await this.Verify(HasSuggestion(transaction, category));
    }

    [Fact]
    public async Task Should_exclude_transaction_not_matching_any_keywords()
    {
        CategoryWithKeywords category = ACategoryMatching("Food");
        this.categoriesDataSource.Feed(category);

        TransactionToCategorize transaction = ATransactionLabeled("Electricity bill");
        this.transactionsToCategorizeDataSource.Feed(transaction);

        await this.Verify(Array.Empty<CategorizationSuggestionPresentation>());
    }
    
    [Fact]
    public async Task Should_exclude_transaction_matching_multiple_categories()
    {
        CategoryWithKeywords fuelCategory = ACategoryMatching("Fuel");
        CategoryWithKeywords cardCategory = ACategoryMatching("Card");
        this.categoriesDataSource.Feed(fuelCategory, cardCategory);

        TransactionToCategorize transaction = ATransactionLabeled("Fuel payment by card");
        this.transactionsToCategorizeDataSource.Feed(transaction);

        await this.Verify(Array.Empty<CategorizationSuggestionPresentation>());
    }
    private async Task Verify(params CategorizationSuggestionPresentation[] expected)
    {
        CategorizationSuggestionPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private static CategoryWithKeywords ACategoryMatching(string keywords) =>
        Any<CategoryWithKeywords>() with { Keywords = keywords };

    private static TransactionToCategorize ATransactionLabeled(string label) =>
        Any<TransactionToCategorize>() with { Label = label };

    private static CategorizationSuggestionPresentation HasSuggestion(TransactionToCategorize transaction, CategoryWithKeywords category) => 
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}