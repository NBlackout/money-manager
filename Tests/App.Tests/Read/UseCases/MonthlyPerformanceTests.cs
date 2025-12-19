using App.Read.Ports;
using App.Read.UseCases;
using App.Shared;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class MonthlyPerformanceTests
{
    private readonly StubbedPeriodProvider periodProvider = new();
    private readonly StubbedPeriodPerformanceDataSource dataSource = new();
    private readonly MonthlyPerformance sut;

    public MonthlyPerformanceTests() =>
        this.sut = new MonthlyPerformance(this.periodProvider, this.dataSource);

    [Theory, RandomData]
    public async Task Gives_performance_of_rolling_twelve_months(Period[] dateRanges, PeriodPerformancePresentation[] expected)
    {
        this.RollingTwelveMonthsAre(dateRanges);
        this.MonthlyPerformanceIs(dateRanges, expected);
        await this.Verify(expected);
    }

    private async Task Verify(PeriodPerformancePresentation[] expected)
    {
        PeriodPerformancePresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void RollingTwelveMonthsAre(Period[] dateRanges) =>
        this.periodProvider.Feed( dateRanges);

    private void MonthlyPerformanceIs(Period[] dateRanges, PeriodPerformancePresentation[] expected) =>
        this.dataSource.Feed(dateRanges, expected);
}

public class StubbedPeriodProvider : IPeriodProvider
{
    private Period[] data = [];

    public Task<Period[]> RollingTwelveMonths() =>
        Task.FromResult(this.data);

    public void Feed(Period[] dateRanges) =>
        this.data = dateRanges;
}