using Write.Infra.Repositories;

namespace Read.Infra.DataSources.TransactionsToCategorize;

public class RepositoryTransactionsToCategorizeDataSource : ITransactionsToCategorizeDataSource
{
    private readonly InMemoryTransactionRepository repository;

    public RepositoryTransactionsToCategorizeDataSource(InMemoryTransactionRepository repository)
    {
        this.repository = repository;
    }

    public Task<TransactionToCategorize[]> Get()
    {
        TransactionToCategorize[] transactions = this.repository.Data
            .Where(t => t.CategoryId is null)
            .Select(t => new TransactionToCategorize(t.Id, t.Label, t.Amount))
            .ToArray();

        return Task.FromResult(transactions);
    }
}