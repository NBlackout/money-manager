using App.Read.Ports;
using App.Shared;
using App.Shared.Ports;
using Tooling;

namespace Infra.Read;

public class PeriodProvider(IClock clock) : IPeriodProvider
{
    public Task<Period[]> RollingTwelveMonths() =>
        Task.FromResult(Enumerable.Range(-11, 12).Select(i => clock.Today.AddMonths(i).MonthRange()).ToArray());
}