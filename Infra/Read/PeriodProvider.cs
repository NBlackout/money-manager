using App.Read.Ports;
using App.Shared;
using App.Shared.Ports;

namespace Infra.Read;

public class PeriodProvider(IClock clock) : IPeriodProvider
{
    public Task<Period[]> RollingTwelveMonths() =>
        Task.FromResult(this.From(-11, 12));

    public Task<Period[]> LastTwelveMonths() =>
        Task.FromResult(this.From(-12, 12));

    public Task<Period[]> NextThreeMonths() =>
        Task.FromResult(this.From(1, 3));

    private Period[] From(int start, int count) =>
        Enumerable.Range(start, count).Select(i => clock.Today.AddMonths(i).MonthRange()).ToArray();
}