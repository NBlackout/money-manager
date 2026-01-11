using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

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
    public async Task Gives_suggestion_on_exact_match()
    {
        CategoryWithKeywords insuranceCategory = ACategoryMatching("Insurance");
        CategoryWithKeywords travelCategory = ACategoryMatching("Travel");
        this.Feed(insuranceCategory, travelCategory);

        TransactionToCategorize insuranceTransaction = ATransactionLabeled("Insurance");
        TransactionToCategorize travelTransaction = ATransactionLabeled("Travel");
        this.Feed(insuranceTransaction, travelTransaction);

        await this.Verify(HasSuggestion(insuranceTransaction, insuranceCategory), HasSuggestion(travelTransaction, travelCategory));
    }

    [Fact]
    public async Task Gives_suggestion_on_partial_match()
    {
        CategoryWithKeywords category = ACategoryMatching("taxi");
        this.Feed(category);

        TransactionToCategorize transaction = ATransactionLabeled("A TAXI fare");
        this.Feed(transaction);

        await this.Verify(HasSuggestion(transaction, category));
    }

    [Fact]
    public async Task Excludes_transaction_not_matching_any_keywords()
    {
        CategoryWithKeywords category = ACategoryMatching("Food");
        this.Feed(category);

        TransactionToCategorize transaction = ATransactionLabeled("Electricity bill");
        this.Feed(transaction);

        await this.Verify();
    }

    [Fact]
    public async Task Excludes_transaction_matching_multiple_categories()
    {
        CategoryWithKeywords fuelCategory = ACategoryMatching("Fuel");
        CategoryWithKeywords cardCategory = ACategoryMatching("Card");
        this.Feed(fuelCategory, cardCategory);

        TransactionToCategorize transaction = ATransactionLabeled("Fuel payment by card");
        this.Feed(transaction);

        await this.Verify();
    }

    [Theory, RandomData]
    public async Task Also_matches_using_amount(decimal amount, TransactionToCategorize transaction)
    {
        CategoryWithKeywords category = ACategory() with { Amount = amount };
        this.Feed(category);
        this.Feed(transaction with { Label = category.Keywords, Amount = AnythingBut(amount) });

        await this.Verify();
    }

    [Theory, InlineData(30, 10, 40), InlineData(30, 10, 20)]
    public async Task Also_matches_using_margin(decimal amount, decimal margin, decimal amount2)
    {
        CategoryWithKeywords category = ACategory() with { Amount = amount, Margin = margin };
        TransactionToCategorize transaction = ATransactionLabeled(category.Keywords) with { Amount = amount2 };
        this.Feed(category);
        this.Feed(transaction);

        await this.Verify(HasSuggestion(transaction, category));
    }

    [Theory, InlineRandomData(30, 10, 41), InlineRandomData(30, 10, 19)]
    public async Task Excludes_transaction_exceeding_amount_margin(
        decimal amount,
        decimal margin,
        decimal amount2,
        CategoryWithKeywords category,
        TransactionToCategorize transaction)
    {
        this.Feed(category with { Amount = amount, Margin = margin });
        this.Feed(transaction with { Label = category.Keywords, Amount = amount2 });

        await this.Verify();
    }

    private async Task Verify(params CategorizationSuggestionPresentation[] expected)
    {
        CategorizationSuggestionPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(params CategoryWithKeywords[] categories) =>
        this.categoriesDataSource.Feed(categories);

    private void Feed(params TransactionToCategorize[] transactions) =>
        this.transactionsToCategorizeDataSource.Feed(transactions);

    private static CategoryWithKeywords ACategoryMatching(string keywords) =>
        ACategory() with { Keywords = keywords };

    private static CategoryWithKeywords ACategory() =>
        Any<CategoryWithKeywords>() with { Amount = null, Margin = null };

    private static TransactionToCategorize ATransactionLabeled(string label) =>
        Any<TransactionToCategorize>() with { Label = label };

    private static CategorizationSuggestionPresentation HasSuggestion(TransactionToCategorize transaction, CategoryWithKeywords category) =>
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}