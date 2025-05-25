namespace Read.App.UseCases;

public class SlidingAccountBalances(ISlidingAccountBalancesDataSource dataSource)
{
    public async Task<SlidingAccountBalancesPresentation> Execute() =>
        await dataSource.All();
}