using App.Read.Ports;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryBalanceDataSource(InMemoryAccountRepository accountRepository, InMemoryTransactionRepository transactionRepository) : IBalanceDataSource
{
    public Task<decimal> On(DateOnly date)
    {
        decimal currentBalance = accountRepository.Data.Sum(a => a.Balance);
        decimal recentDelta = transactionRepository.Data.Where(t => t.Date >= date).Sum(t => t.Amount);

        return Task.FromResult(currentBalance - recentDelta);
    }
}