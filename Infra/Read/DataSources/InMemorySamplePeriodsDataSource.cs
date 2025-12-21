using App.Read.Ports;
using App.Shared;
using App.Write.Model.Transactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemorySamplePeriodsDataSource(InMemoryTransactionRepository transactionRepository) : ISamplePeriodsDataSource
{
    public Task<SamplePeriod[]> Of(Period[] periods)
    {
        TransactionSnapshot[] transactions = transactionRepository.Data.ToArray();
        SamplePeriod[] forecastPeriods = periods.Select(p => new SamplePeriod(p, transactions.Where(t => p.Includes(t.Date)).Sum(t => t.Amount))).ToArray();

        return Task.FromResult(forecastPeriods);
    }
}