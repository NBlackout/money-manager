using App.Shared;

namespace App.Read.Ports;

public interface IPeriodPerformanceDataSource
{
    Task<PeriodPerformancePresentation[]> All(params Period[] dateRanges);
}

public record PeriodPerformancePresentation(Period Period, decimal Balance, PerformancePresentation Performance);

public sealed record PerformancePresentation(decimal Revenue, decimal Expenses, decimal Net);