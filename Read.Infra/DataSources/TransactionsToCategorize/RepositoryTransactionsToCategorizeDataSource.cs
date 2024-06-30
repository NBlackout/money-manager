using Write.Infra.Repositories;

namespace Read.Infra.DataSources.TransactionsToCategorize;

public class RepositoryTransactionsToCategorizeDataSource(InMemoryTransactionRepository repository)
    : ITransactionsToCategorizeDataSource
{
    public Task<TransactionToCategorize[]> All()
    {
        TransactionToCategorize[] transactions = repository.Data
            .Where(t => t.CategoryId is null)
            .Select(t => new TransactionToCategorize(t.Id, t.Label, t.Amount))
            .ToArray();

        return Task.FromResult(transactions);
    }
}
