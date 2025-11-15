using App.Read.Ports;
using App.Write.Model.Transactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryPerformanceForecastDataSource(InMemoryAccountRepository accountRepository, InMemoryTransactionRepository transactionRepository)
    : IPerformanceForecastDataSource
{
    public Task<PerformanceForecastPresentation> Fetch()
    {
        decimal totalBalance = accountRepository.Data.Sum(a => a.Balance);

        TransactionSnapshot[] recurringTransactions = [..transactionRepository.Data.Where(t => t.IsRecurring)];
        PerformancePresentation performance = recurringTransactions.Aggregate(
            new PerformancePresentation(0, 0, 0),
            (p, t) => new PerformancePresentation(p.Revenue + Math.Max(t.Amount, 0), p.Expenses + Math.Min(t.Amount, 0), p.Net + t.Amount)
        );

        return Task.FromResult(new PerformanceForecastPresentation(totalBalance, performance.Revenue, performance.Expenses, performance.Net));
    }
}