namespace Read.App.UseCases;

public class CategorizationSuggestions
{
    private readonly ICategoriesWithKeywordsDataSource categoriesDataSource;
    private readonly ITransactionsToCategorizeDataSource transactionsToCategorizeDataSource;

    public CategorizationSuggestions(ICategoriesWithKeywordsDataSource categoriesDataSource,
        ITransactionsToCategorizeDataSource transactionsToCategorizeDataSource)
    {
        this.categoriesDataSource = categoriesDataSource;
        this.transactionsToCategorizeDataSource = transactionsToCategorizeDataSource;
    }

    public async Task<CategorizationSuggestionPresentation[]> Execute()
    {
        CategoryWithKeywords[] categories = await this.categoriesDataSource.Get();
        TransactionToCategorize[] transactions = await this.transactionsToCategorizeDataSource.Get();

        return Match(transactions, categories);
    }

    private static CategorizationSuggestionPresentation[] Match(TransactionToCategorize[] transactions,
        CategoryWithKeywords[] categories) =>
        transactions.Select(t => Match(t, categories)).Where(suggestion => suggestion is not null).ToArray()!;

    private static CategorizationSuggestionPresentation? Match(TransactionToCategorize transaction,
        CategoryWithKeywords[] categories)
    {
        CategoryWithKeywords[] matchingCategories = categories
            .Where(c => transaction.Label.Contains(c.Keywords, StringComparison.InvariantCultureIgnoreCase)).ToArray();

        return matchingCategories.Length == 1 ? Match(transaction, matchingCategories.Single()) : null;
    }

    private static CategorizationSuggestionPresentation Match(TransactionToCategorize transaction, CategoryWithKeywords category) =>
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}