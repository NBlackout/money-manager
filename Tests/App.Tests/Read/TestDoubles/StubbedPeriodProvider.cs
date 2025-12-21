using App.Read.Ports;
using App.Shared;

namespace App.Tests.Read.TestDoubles;

public class StubbedPeriodProvider : IPeriodProvider
{
    private Period[] rollingTwelveMonths = [];
    private Period[] lastTwelveMonths = [];
    private Period[] nextThreeMonths = [];

    public Task<Period[]> RollingTwelveMonths() =>
        Task.FromResult(this.rollingTwelveMonths);

    public Task<Period[]> LastTwelveMonths() =>
        Task.FromResult(this.lastTwelveMonths);

    public Task<Period[]> NextThreeMonths() =>
        Task.FromResult(this.nextThreeMonths);

    public void FeedRollingTwelveMonths(Period[] periods) =>
        this.rollingTwelveMonths = periods;

    public void FeedLastTwelveMonths(Period[] periods) =>
        this.lastTwelveMonths = periods;

    public void FeedNextThreeMonths(Period[] periods) =>
        this.nextThreeMonths = periods;
}