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
        CategoryWithKeywords[] matchingCategories = [..categories.Where(c => Matches(transaction, c))];

        return matchingCategories.Length == 1 ? PresentationFrom(transaction, matchingCategories[0]) : null;
    }

    private static bool Matches(TransactionToCategorize transaction, CategoryWithKeywords category)
    {
        if (transaction.Label.Contains(category.Keywords, StringComparison.InvariantCultureIgnoreCase) is false)
            return false;
        if (category.Amount.HasValue is false)
            return true;
        if (category.Margin.HasValue is false)
            return transaction.Amount == category.Amount.Value;
        if (transaction.Amount < category.Amount - category.Margin)
            return false;
        if (transaction.Amount > category.Amount + category.Margin)
            return false;

        return true;
    }

    private static CategorizationSuggestionPresentation PresentationFrom(TransactionToCategorize transaction, CategoryWithKeywords category) =>
        new(transaction.Id, transaction.Label, transaction.Amount, category.Id, category.Label);
}

public record CategorizationSuggestionPresentation(
    Guid TransactionId,
    string TransactionLabel,
    decimal TransactionAmount,
    Guid CategoryId,
    string CategoryLabel
);