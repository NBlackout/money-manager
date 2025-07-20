using App.Read.Ports;
using App.Write.Model.Accounts;
using App.Write.Model.Transactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryTransactionsOfMonthDataSource(InMemoryTransactionRepository transactionRepository, InMemoryCategoryRepository categoryRepository)
    : ITransactionsOfMonthDataSource
{
    public Task<TransactionSummaryPresentation[]> By(Guid accountId, int year, int month)
    {
        TransactionSummaryPresentation[] presentations =
        [
            ..transactionRepository
                .Data
                .Where(t => t.AccountId == new AccountId(accountId) && t.Date.Year == year && t.Date.Month == month)
                .Select(this.ToPresentation)
        ];

        return Task.FromResult(presentations);
    }

    private TransactionSummaryPresentation ToPresentation(TransactionSnapshot transaction)
    {
        string? categoryLabel = transaction.CategoryId is not null ? categoryRepository.Data.Single(c => c.Id == transaction.CategoryId).Label : null;

        return new TransactionSummaryPresentation(transaction.Id.Value, transaction.Amount, transaction.Label, transaction.Date, categoryLabel);
    }
}