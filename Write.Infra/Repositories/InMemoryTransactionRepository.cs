namespace Write.Infra.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly Dictionary<Guid, Transaction> data = new();
    private readonly List<string> knownExternalIds = new();

    public IEnumerable<TransactionSnapshot> Data => this.data.Values.Select(t => t.Snapshot);
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Transaction> By(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task<string[]> UnknownExternalIds(IEnumerable<string> externalIds)
    {
        string[] unknownExternalIds = externalIds.Except(this.knownExternalIds).ToArray();

        return Task.FromResult(unknownExternalIds);
    }

    public Task Save(Transaction transaction)
    {
        TransactionSnapshot snapshot = transaction.Snapshot;
        this.data[transaction.Id] = Transaction.From(snapshot);
        this.knownExternalIds.Add(snapshot.ExternalId);

        return Task.CompletedTask;
    }

    public void Feed(params Transaction[] transactions) =>
        transactions.ToList().ForEach(transaction => this.Save(transaction));

    public void Clear() =>
        this.data.Clear();
}