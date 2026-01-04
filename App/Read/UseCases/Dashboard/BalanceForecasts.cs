using App.Read.Ports;
using App.Shared;
using App.Shared.Ports;

namespace App.Read.UseCases.Dashboard;

public class BalanceForecasts(
    IClock clock,
    IPeriodProvider periodProvider,
    IBalanceDataSource balanceDataSource,
    ISamplePeriodsDataSource samplePeriodsDataSource,
    IForecastPeriodsDataSource forecastPeriodsDataSource,
    IBalanceForecastsDataSource balanceForecastsDataSource
)
{
    public async Task<BalanceForecastPresentation[]> Execute()
    {
        decimal balance = await this.BalanceOnFirstDayOfMonth();
        SamplePeriod[] samplePeriods = await this.LastTwelveMonthsTransactions();
        ForecastPeriod[] forecastPeriods = await this.NextThreeMonthsTransactions();

        return await balanceForecastsDataSource.Fetch(balance, samplePeriods, forecastPeriods);
    }

    private async Task<decimal> BalanceOnFirstDayOfMonth() =>
        await balanceDataSource.On(clock.FirstDayOfMonth);

    private async Task<SamplePeriod[]> LastTwelveMonthsTransactions()
    {
        Period[] lastTwelveMonths = await periodProvider.LastTwelveMonths();

        return await samplePeriodsDataSource.Of(lastTwelveMonths);
    }

    private async Task<ForecastPeriod[]> NextThreeMonthsTransactions()
    {
        Period[] nextThreeMonths = await periodProvider.NextThreeMonths();

        return await forecastPeriodsDataSource.Of(nextThreeMonths);
    }
}