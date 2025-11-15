using App.Read.Ports;

namespace App.Read.UseCases;

public class PerformanceForecast(IPerformanceForecastDataSource dataSource)
{
    public async Task<PerformanceForecastPresentation> Execute() =>
        await dataSource.Fetch();
}