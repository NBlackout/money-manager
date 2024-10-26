namespace Read.App.UseCases;

public class CategorizationSuggestions(
    ICategoriesWithKeywordsDataSource categoriesDataSource,
    ITransactionsToCategorizeDataSource transactionsToCategorizeDataSource)
{
    public async Task<CategorizationSuggestionPresentation[]> Execute()
    {
        CategoryWithKeywords[] categories = await categoriesDataSource.All();
        TransactionToCategorize[] transactions = await transactionsToCategorizeDataSource.All();

        return Match(transactions, categories);
    }

    private static CategorizationSuggestionPresentation[] Match(TransactionToCategorize[] transactions,
        CategoryWithKeywords[] categories)
    {
        return [..transactions.Select(t => Match(t, categories)).Where(suggestion => suggestion is not null)!];
    }

    private static CategorizationSuggestionPresentation? Match(TransactionToCategorize transaction,
        CategoryWithKeywords[] categories)
    {
        CategoryWithKeywords[] matchingCategories =
        [
            ..categories
                .Where(c => transaction.Label.Contains(c.Keywords, StringComparison.InvariantCultureIgnoreCase))
        ];

        return matchingCategories.Length == 1 ? Match(transaction, matchingCategories.Single()) : null;
    }

    private static CategorizationSuggestionPresentation Match(TransactionToCategorize transaction,
        CategoryWithKeywords category) =>
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}