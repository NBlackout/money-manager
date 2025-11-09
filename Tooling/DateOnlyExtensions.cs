using App.Shared;

namespace Tooling;

public static class DateOnlyExtensions
{
    public static DateRange MonthRange(this DateOnly date) =>
        new(date.FirstDayOfMonth(), date.LastDayOfMonth());

    private static DateOnly FirstDayOfMonth(this DateOnly date) =>
        new(date.Year, date.Month, 1);

    private static DateOnly LastDayOfMonth(this DateOnly date) =>
        date.FirstDayOfMonth().AddMonths(1).AddDays(-1);
}