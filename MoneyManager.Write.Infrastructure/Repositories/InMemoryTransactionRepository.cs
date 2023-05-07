namespace MoneyManager.Write.Infrastructure.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly Dictionary<Guid, Transaction> data = new();
    private readonly List<string> knownExternalIds = new();

    public IEnumerable<Transaction> Data => this.data.Values;
    public Func<Guid> NextId { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Transaction> ById(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task<IReadOnlyCollection<string>> UnknownExternalIds(IEnumerable<string> externalIds)
    {
        IReadOnlyCollection<string> unknownExternalIds = externalIds.Except(this.knownExternalIds).ToList();

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