using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;
using Shared.TestTooling.TestDoubles;

namespace App.Tests.Read.UseCases;

public class SlidingBalancesTests
{
    private readonly StubbedSlidingBalancesDataSource dataSource = new();
    private readonly StubbedClock clock = new();
    private readonly SlidingBalances sut;

    public SlidingBalancesTests()
    {
        this.sut = new SlidingBalances(this.dataSource, this.clock);
    }

    [Theory]
    [InlineRandomData("2025-08-13", "2025-08-01", "2025-02-01")]
    [InlineRandomData("2028-11-02", "2028-11-01", "2028-05-01")]
    public async Task Gives_sliding_balances_from_start_of_month(string today, string startOfMonth, string startingFrom, SlidingBalancesPresentation expected)
    {
        this.TodayIs(DateOnly.Parse(today));
        this.SetSlidingBalancesTo(DateOnly.Parse(startOfMonth), DateOnly.Parse(startingFrom), expected);
        await this.Verify(expected);
    }

    private async Task Verify(SlidingBalancesPresentation expected)
    {
        SlidingBalancesPresentation actual = await this.sut.Execute();
        actual.Should().BeEquivalentTo(expected);
    }

    private void TodayIs(DateOnly today) =>
        this.clock.Today = today;

    private void SetSlidingBalancesTo(DateOnly baseline, DateOnly startingFrom, SlidingBalancesPresentation expected) =>
        this.dataSource.Feed(baseline, startingFrom, expected);
}