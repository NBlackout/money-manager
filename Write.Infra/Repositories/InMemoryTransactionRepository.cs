namespace Write.Infra.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly Dictionary<Guid, TransactionSnapshot> data = new();
    private readonly List<string> knownExternalIds = [];

    public IEnumerable<TransactionSnapshot> Data => this.data.Values.Select(t => t);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Transaction> By(Guid id) =>
        Task.FromResult(Transaction.From(this.data[id]));

    public Task<string[]> UnknownExternalIds(IEnumerable<string> externalIds)
    {
        string[] unknownExternalIds = externalIds.Except(this.knownExternalIds).ToArray();

        return Task.FromResult(unknownExternalIds);
    }

    public Task Save(Transaction transaction)
    {
        TransactionSnapshot snapshot = transaction.Snapshot;
        this.data[transaction.Id] = snapshot;
        this.knownExternalIds.Add(snapshot.ExternalId);

        return Task.CompletedTask;
    }

    public void Feed(params TransactionSnapshot[] transactions) =>
        transactions.ToList().ForEach(transaction => this.data[transaction.Id] = transaction);
}