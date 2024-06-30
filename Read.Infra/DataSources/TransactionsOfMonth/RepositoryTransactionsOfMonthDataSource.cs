using Write.App.Model.Transactions;
using Write.Infra.Repositories;

namespace Read.Infra.DataSources.TransactionsOfMonth;

public class RepositoryTransactionsOfMonthDataSource(
    InMemoryTransactionRepository transactionRepository,
    InMemoryCategoryRepository categoryRepository)
    : ITransactionsOfMonthDataSource
{
    public Task<TransactionSummaryPresentation[]> Get(Guid accountId, int year, int month)
    {
        TransactionSummaryPresentation[] presentations = transactionRepository.Data
            .Where(t => t.AccountId == accountId && t.Date.Year == year && t.Date.Month == month)
            .Select(this.ToPresentation)
            .ToArray();

        return Task.FromResult(presentations);
    }

    private TransactionSummaryPresentation ToPresentation(TransactionSnapshot transaction)
    {
        string? categoryLabel = transaction.CategoryId.HasValue
            ? categoryRepository.Data.Single(c => c.Id == transaction.CategoryId.Value).Label
            : null;

        return new TransactionSummaryPresentation(transaction.Id, transaction.Amount, transaction.Label,
            transaction.Date, categoryLabel);
    }
}
