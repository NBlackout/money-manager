using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.TransactionsOfMonth;

public class RepositoryTransactionsOfMonthDataSource : ITransactionsOfMonthDataSource
{
    private readonly InMemoryTransactionRepository repository;

    public RepositoryTransactionsOfMonthDataSource(InMemoryTransactionRepository repository)
    {
        this.repository = repository;
    }

    public Task<IReadOnlyCollection<TransactionSummaryPresentation>> Get(Guid accountId, int year, int month)
    {
        IReadOnlyCollection<TransactionSummaryPresentation> presentations = this.repository.Data
            .Where(t => t.AccountId == accountId && t.Date.Year == year && t.Date.Month == month)
            .Select(t => new TransactionSummaryPresentation(t.Id, t.Amount, t.Label, t.Date))
            .ToList();

        return Task.FromResult(presentations);
    }
}