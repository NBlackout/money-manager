using App.Shared;

namespace App.Read.Ports;

public interface IPeriodPerformanceDataSource
{
    Task<PeriodPerformancePresentation[]> All(params DateRange[] dateRanges);
}

public record PeriodPerformancePresentation(DateRange DateRange, decimal Balance, PerformancePresentation Performance);

public sealed record PerformancePresentation(decimal Revenue, decimal Expenses, decimal Net);