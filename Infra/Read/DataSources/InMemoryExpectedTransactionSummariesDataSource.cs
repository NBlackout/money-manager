using App.Read.Ports;
using App.Write.Model.Transactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryExpectedTransactionSummariesDataSource(InMemoryTransactionRepository repository) : IExpectedTransactionSummariesDataSource
{
    public Task<ExpectedTransactionSummaryPresentation[]> All() =>
        Task.FromResult(repository.Data.Where(t => t.IsRecurring).Select(PresentationFrom).ToArray());

    private static ExpectedTransactionSummaryPresentation PresentationFrom(TransactionSnapshot transaction) =>
        new(transaction.Amount, transaction.Label);
}