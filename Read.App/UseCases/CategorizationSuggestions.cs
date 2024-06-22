namespace Read.App.UseCases;

public class CategorizationSuggestions
{
    private readonly ICategoriesWithPatternDataSource categoriesDataSource;
    private readonly ITransactionsToCategorizeDataSource transactionsToCategorizeDataSource;

    public CategorizationSuggestions(ICategoriesWithPatternDataSource categoriesDataSource,
        ITransactionsToCategorizeDataSource transactionsToCategorizeDataSource)
    {
        this.categoriesDataSource = categoriesDataSource;
        this.transactionsToCategorizeDataSource = transactionsToCategorizeDataSource;
    }

    public async Task<CategorizationSuggestionPresentation[]> Execute()
    {
        CategoryWithPattern[] categories = await this.categoriesDataSource.Get();
        TransactionToCategorize[] transactions = await this.transactionsToCategorizeDataSource.Get();

        return Match(transactions, categories);
    }

    private static CategorizationSuggestionPresentation[] Match(TransactionToCategorize[] transactions,
        CategoryWithPattern[] categories) =>
        transactions.Select(t => Match(t, categories)).Where(suggestion => suggestion is not null).ToArray()!;

    private static CategorizationSuggestionPresentation? Match(TransactionToCategorize transaction,
        CategoryWithPattern[] categories)
    {
        CategoryWithPattern[] matchingCategories = categories
            .Where(c => transaction.Label.Contains(c.Pattern, StringComparison.InvariantCultureIgnoreCase)).ToArray();

        return matchingCategories.Length == 1 ? Match(transaction, matchingCategories.Single()) : null;
    }

    private static CategorizationSuggestionPresentation Match(TransactionToCategorize transaction, CategoryWithPattern category) =>
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}