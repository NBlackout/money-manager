using App.Read.Ports;
using App.Shared.Ports;

namespace App.Read.UseCases;

public class SlidingBalances(ISlidingBalancesDataSource dataSource, IClock clock)
{
    public async Task<SlidingBalancesPresentation> Execute()
    {
        DateOnly startingOfThisMonth = new(clock.Today.Year, clock.Today.Month, 1);

        return await dataSource.All(startingOfThisMonth, startingOfThisMonth.AddMonths(-6));
    }
}