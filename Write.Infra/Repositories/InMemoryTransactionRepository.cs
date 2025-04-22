using Write.App.Model.ValueObjects;

namespace Write.Infra.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly Dictionary<TransactionId, TransactionSnapshot> data = new();

    public IEnumerable<TransactionSnapshot> Data => this.data.Values.Select(t => t);
    public Func<TransactionId> NextId { get; set; } = null!;

    public Task<TransactionId> NextIdentity() =>
        Task.FromResult(this.NextId());

    public Task<Transaction> By(TransactionId id) =>
        Task.FromResult(Transaction.From(this.data[id]));

    public Task<Transaction[]> By(CategoryId categoryId) =>
        Task.FromResult(this.data.Values.Where(t => t.CategoryId == categoryId).Select(Transaction.From).ToArray());

    public Task<ExternalId[]> UnknownExternalIds(IEnumerable<ExternalId> externalIds) =>
        Task.FromResult(externalIds.Except(this.data.Values.Select(v => new ExternalId(v.ExternalId))).ToArray());

    public Task Save(params Transaction[] transactions)
    {
        foreach (Transaction transaction in transactions)
            this.data[transaction.Id] = transaction.Snapshot;

        return Task.CompletedTask;
    }

    public void Feed(params TransactionSnapshot[] transactions) =>
        transactions.ToList().ForEach(transaction => this.data[transaction.Id] = transaction);
}