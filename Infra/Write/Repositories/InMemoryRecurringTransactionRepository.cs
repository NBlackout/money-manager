using App.Write.Model.RecurringTransactions;
using App.Write.Ports;

namespace Infra.Write.Repositories;

public class InMemoryRecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly Dictionary<RecurringTransactionId, RecurringTransactionSnapshot> data = new();

    public IEnumerable<RecurringTransactionSnapshot> Data => this.data.Values.Select(t => t);

    public Task<RecurringTransaction> By(RecurringTransactionId id) =>
        Task.FromResult(RecurringTransaction.From(this.data[id]));

    public Task Save(RecurringTransaction recurringTransaction)
    {
        this.data[recurringTransaction.Id] = recurringTransaction.Snapshot;

        return Task.CompletedTask;
    }

    public void Feed(RecurringTransactionSnapshot[] recurringTransactions)
    {
        foreach (RecurringTransactionSnapshot recurringTransaction in recurringTransactions)
            this.data[recurringTransaction.Id] = recurringTransaction;
    }
}