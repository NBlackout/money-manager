using App.Read.Ports;
using App.Shared;
using App.Shared.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Microsoft.Extensions.DependencyInjection;
using TestTooling.TestDoubles;

namespace Infra.Tests.Read.DataSources;

public class InMemoryBalanceForecastsDataSourceTests : InfraTest<IBalanceForecastsDataSource, InMemoryBalanceForecastsDataSource>
{
    private static readonly DateOnly LastDayOfMonth = Any<DateOnly>();

    private readonly StubbedClock clock = new();

    public InMemoryBalanceForecastsDataSourceTests()
    {
        this.clock.LastDayOfMonth = LastDayOfMonth;
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddSingleton<IClock>(this.clock);

    [Theory, RandomData]
    public async Task Forecasts_balance_when_there_is_no_activity(decimal balance) =>
        await this.Verify(balance, ExpectedEndOfMonthBalance(balance));

    [Theory, RandomData]
    public async Task Gives_forecasts_based_on_recurring_transactions(Period aPeriod, Period anotherPeriod)
    {
        ForecastPeriod[] forecastPeriods = [new(aPeriod, -250), new(anotherPeriod, -100)];
        await this.Verify(
            150,
            forecastPeriods,
            ExpectedEndOfMonthBalance(150),
            new BalanceForecastPresentation(aPeriod.To, -100),
            new BalanceForecastPresentation(anotherPeriod.To, -200)
        );
    }

    [Theory, RandomData]
    public async Task Gives_forecasts_based_on_average_amount(Period aPeriod, Period anotherPeriod)
    {
        SamplePeriod[] samplePeriods = [new(aPeriod, -1000.50m), new(anotherPeriod, -2000.25m)];
        await this.Verify(1000, samplePeriods, ExpectedEndOfMonthBalance(-500.38m));
    }

    private async Task Verify(decimal balance, params BalanceForecastPresentation[] expected) =>
        await this.Verify(balance, [], [], expected);

    private async Task Verify(decimal balance, ForecastPeriod[] forecastPeriods, params BalanceForecastPresentation[] expected) =>
        await this.Verify(balance, [], forecastPeriods, expected);

    private async Task Verify(decimal balance, SamplePeriod[] samplePeriods, params BalanceForecastPresentation[] expected) =>
        await this.Verify(balance, samplePeriods, [], expected);

    private async Task Verify(decimal balance, SamplePeriod[] samplePeriods, ForecastPeriod[] forecastPeriods, params BalanceForecastPresentation[] expected)
    {
        BalanceForecastPresentation[] actual = await this.Sut.Fetch(balance, samplePeriods, forecastPeriods);
        actual.Should().Equal(expected);
    }

    private static BalanceForecastPresentation ExpectedEndOfMonthBalance(decimal balance) =>
        new(LastDayOfMonth, balance);
}