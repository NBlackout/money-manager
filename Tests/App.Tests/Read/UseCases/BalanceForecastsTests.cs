using App.Read.Ports;
using App.Read.UseCases;
using App.Shared;
using App.Tests.Read.TestDoubles;
using TestTooling.TestDoubles;

namespace App.Tests.Read.UseCases;

public class BalanceForecastsTests
{
    private readonly StubbedClock clock = new();
    private readonly StubbedPeriodProvider periodProvider = new();
    private readonly StubbedBalanceDataSource balanceDataSource = new();
    private readonly StubbedSamplePeriodsDataSource samplePeriodsDataSource = new();
    private readonly StubbedForecastPeriodsDataSource forecastPeriodsDataSource = new();
    private readonly StubbedBalanceForecastsDataSource balanceForecastsDataSource = new();
    private readonly BalanceForecasts sut;

    public BalanceForecastsTests() =>
        this.sut = new BalanceForecasts(
            this.clock,
            this.periodProvider,
            this.balanceDataSource,
            this.samplePeriodsDataSource,
            this.forecastPeriodsDataSource,
            this.balanceForecastsDataSource
        );

    [Theory]
    [RandomData]
    public async Task Gives_balance_forecasts(
        DateOnly firstDayOfMonth,
        decimal balance,
        Period[] rollingTwelveMonths,
        SamplePeriod[] sampleTransactions,
        Period[] nextThreeMonths,
        ForecastPeriod[] forecastPeriods,
        BalanceForecastPresentation[] balanceForecasts)
    {
        this.SetBalanceOn(firstDayOfMonth, balance);
        this.SetSampleTransactionsTo(rollingTwelveMonths, sampleTransactions);
        this.SetScheduledTransactionsTo(nextThreeMonths, forecastPeriods);
        this.SetForecastsTo(balance, sampleTransactions, forecastPeriods, balanceForecasts);

        await this.Verify(balanceForecasts);
    }

    private async Task Verify(params BalanceForecastPresentation[] expected)
    {
        BalanceForecastPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void SetBalanceOn(DateOnly firstDayOfMonth, decimal balance)
    {
        this.clock.FirstDayOfMonth = firstDayOfMonth;
        this.balanceDataSource.Feed(firstDayOfMonth, balance);
    }

    private void SetSampleTransactionsTo(Period[] lastTwelveMonths, params SamplePeriod[] transactions)
    {
        this.periodProvider.FeedLastTwelveMonths(lastTwelveMonths);
        this.samplePeriodsDataSource.Feed(lastTwelveMonths, transactions);
    }

    private void SetScheduledTransactionsTo(Period[] nextThreeMonths, params ForecastPeriod[] transactions)
    {
        this.periodProvider.FeedNextThreeMonths(nextThreeMonths);
        this.forecastPeriodsDataSource.Feed(nextThreeMonths, transactions);
    }

    private void SetForecastsTo(
        decimal balance,
        SamplePeriod[] sampleTransactions,
        ForecastPeriod[] forecastPeriods,
        params BalanceForecastPresentation[] expected) =>
        this.balanceForecastsDataSource.Feed(balance, sampleTransactions, forecastPeriods, expected);
}