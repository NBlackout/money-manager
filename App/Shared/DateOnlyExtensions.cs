namespace App.Shared;

public static class DateOnlyExtensions
{
    public static Period MonthRange(this DateOnly date) =>
        new(date.FirstDayOfMonth(), date.LastDayOfMonth());

    public static DateOnly FirstDayOfMonth(this DateOnly date) =>
        new(date.Year, date.Month, 1);

    public static DateOnly LastDayOfMonth(this DateOnly date) =>
        date.FirstDayOfMonth().AddMonths(1).AddDays(-1);
}