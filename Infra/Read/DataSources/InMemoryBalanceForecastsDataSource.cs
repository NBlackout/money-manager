using App.Read.Ports;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryBalanceForecastsDataSource(
    InMemoryAccountRepository accountRepository,
    InMemoryRecurringTransactionRepository recurringTransactionRepository
) : IBalanceForecastsDataSource
{
    public Task<BalanceForecastPresentation[]> Fetch()
    {
        decimal totalBalance = accountRepository.Data.Sum(a => a.Balance);
        decimal recurringNet = recurringTransactionRepository.Data.Sum(t => t.Amount);

        return Task.FromResult(
            new BalanceForecastPresentation[]
            {
                new(totalBalance + recurringNet * 1),
                new(totalBalance + recurringNet * 2),
                new(totalBalance + recurringNet * 3),
                new(totalBalance + recurringNet * 4)
            }
        );
    }
}