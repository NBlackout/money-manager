namespace MoneyManager.Write.Infrastructure.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly Dictionary<Guid, Transaction> data = new();
    private readonly List<string> knownExternalIds = new();
    private int nextIdIndex;

    public Func<Guid>[] NextIds { get; set; } = null!;

    public Task<Guid> NextIdentity() =>
        Task.FromResult(this.NextIds[this.nextIdIndex++]());

    public Task<Transaction> GetById(Guid id) =>
        Task.FromResult(this.data[id]);

    public Task<IReadOnlyCollection<string>> GetUnknownExternalIds(IEnumerable<string> externalIds)
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
}