using App.Shared;

namespace Infra.Shared.Extensions;

public static class PeriodExtensions
{
    public static bool Includes(this Period period, DateOnly date) =>
        date >= period.From && date <= period.To;
}