using App.Read.Ports;

namespace App.Read.UseCases;

public class BalanceForecasts(IBalanceForecastsDataSource dataSource)
{
    public async Task<BalanceForecastPresentation[]> Execute() =>
        await dataSource.Fetch();
}