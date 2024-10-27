namespace Read.Infra.DataSources;

public class InMemoryTransactionsToCategorizeDataSource(InMemoryTransactionRepository repository)
    : ITransactionsToCategorizeDataSource
{
    public Task<TransactionToCategorize[]> All()
    {
        TransactionToCategorize[] transactions =
        [
            ..repository
                .Data
                .Where(t => t.CategoryId is null)
                .Select(t => new TransactionToCategorize(t.Id.Value, t.Label, t.Amount))
        ];

        return Task.FromResult(transactions);
    }
}