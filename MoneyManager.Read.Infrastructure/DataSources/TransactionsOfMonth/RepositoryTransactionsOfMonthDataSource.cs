using MoneyManager.Write.Application.Model.Transactions;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.TransactionsOfMonth;

public class RepositoryTransactionsOfMonthDataSource : ITransactionsOfMonthDataSource
{
    private readonly InMemoryTransactionRepository transactionRepository;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryTransactionsOfMonthDataSource(InMemoryTransactionRepository transactionRepository,
        InMemoryCategoryRepository categoryRepository)
    {
        this.transactionRepository = transactionRepository;
        this.categoryRepository = categoryRepository;
    }

    public Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid accountId, int year, int month)
    {
        IReadOnlyCollection<TransactionSummaryPresentation> presentations = this.transactionRepository.Data
            .Where(t => t.AccountId == accountId && t.Date.Year == year && t.Date.Month == month)
            .Select(this.ToPresentation)
            .ToList();

        return Task.FromResult(presentations);
    }

    private TransactionSummaryPresentation ToPresentation(TransactionSnapshot transaction)
    {
        string? categoryLabel = transaction.CategoryId.HasValue
            ? this.categoryRepository.Data.Single(c => c.Id == transaction.CategoryId.Value).Label
            : null;

        return new TransactionSummaryPresentation(transaction.Id, transaction.Amount, transaction.Label,
            transaction.Date, categoryLabel);
    }
}