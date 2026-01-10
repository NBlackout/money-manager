using App.Read.Ports;

namespace App.Read.UseCases;

public class CategorizationSuggestions(
    ICategoriesWithKeywordsDataSource categoriesDataSource,
    ITransactionsToCategorizeDataSource transactionsToCategorizeDataSource
)
{
    public async Task<CategorizationSuggestionPresentation[]> Execute()
    {
        CategoryWithKeywords[] categories = await categoriesDataSource.All();
        TransactionToCategorize[] transactions = await transactionsToCategorizeDataSource.All();

        return Match(transactions, categories);
    }

    private static CategorizationSuggestionPresentation[] Match(TransactionToCategorize[] transactions, CategoryWithKeywords[] categories) =>
    [
        ..transactions.Select(t => Match(t, categories)).Where(s => s is not null)!
    ];

    private static CategorizationSuggestionPresentation? Match(TransactionToCategorize transaction, CategoryWithKeywords[] categories)
    {
        CategoryWithKeywords[] matchingCategories =
        [
            ..categories.Where(c =>
                transaction.Label.Contains(c.Keywords, StringComparison.InvariantCultureIgnoreCase) &&
                (!c.Amount.HasValue || transaction.Amount == c.Amount.Value)
            )
        ];

        return matchingCategories.Length == 1 ? Match(transaction, matchingCategories[0]) : null;
    }

    private static CategorizationSuggestionPresentation Match(TransactionToCategorize transaction, CategoryWithKeywords category) =>
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}

public record CategorizationSuggestionPresentation(
    Guid TransactionId,
    string TransactionLabel,
    decimal TransactionAmount,
    Guid CategoryId,
    string CategoryLabel
);